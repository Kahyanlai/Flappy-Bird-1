using System;
using SplashKitSDK;

namespace Flappybird
{
     public class Program
    {
        private const string HighScoreFileName = "highscore.txt"; //file name to store the high score

        public static void Main()
        {
            Window window = new Window("Flappy Bird", 800, 600); // Create a new game window

            int highScore = LoadHighScore(); //load high score from file

            HomePage homePage = new HomePage(window, highScore); //create a new window for home page
            bool startGame = false;

            //display home page until the game starts
            while (!window.CloseRequested && !startGame)
            {
                homePage.HandleInput(); //handle input on the home page
                homePage.Draw(); //draw the home page
                startGame = homePage.ShouldStartGame(); //check if the game should start
            }

            //main game loop
            if (startGame)
            {
                FlappyBirdGame game = new FlappyBirdGame(window, highScore); //create a new window for the game 
                game.Run(); //run the game
            }
        }

        //method to load the high score from a file
        private static int LoadHighScore()
        {
            if (File.Exists(HighScoreFileName)) //check if the file exists
            {
                string highScoreText = File.ReadAllText(HighScoreFileName); //read the file content
                if (int.TryParse(highScoreText, out int highScore)) //try to parse the content as an integer
                {
                    return highScore; //return the parsed high score
                }
            }
            return 0; //return 0 if the file does not exist or parsing fails
        }
    }

    public class FlappyBirdGame
    {
        private Window _window;
        private FlappyBird _bird;
        private List<Pipe> _pipes;
        private SplashKitSDK.Timer _gameTimer; 
        private double _pipeSpawnInterval;
        private int _highScore;
        private Bitmap _gameBackgroundBitmap;

        // Constructor to initialize the game
        public FlappyBirdGame(Window window, int highScore)
        {
            _window = window;
            _highScore = highScore;
            _bird = new FlappyBird(); // create a new object of the flappy bird
            _pipes = new List<Pipe>(); // create a new list to store pipe
            _gameTimer = new SplashKitSDK.Timer("gameTimer"); // initialize the game timer
            _pipeSpawnInterval = 2000; // interval for spawning new pipes eash 2 second
            _gameTimer.Start(); // start the game timer
            _gameBackgroundBitmap = SplashKit.LoadBitmap("gameBackground", "bb.png");
        }

        // method to run the game
        public void Run()
        {
            while (!_window.CloseRequested) //game loop
            {
                SplashKit.ProcessEvents();

                if (_bird.IsAlive()) //check if bird is alive
                {
                    _bird.Update(_pipes); //update bird position and check colliison

                    if (_gameTimer.Ticks > _pipeSpawnInterval) //check if the current time exceeded the pipe spawn interval
                    {
                        _pipes.Add(new Pipe(800)); //new pipe object is created with X-coordinate of 800 
                        _gameTimer.Reset();
                    }

                    //update pipes and check for collisions
                    foreach (Pipe pipe in _pipes)
                    {
                        pipe.Update(); // update pipes position

                        if (_bird.CheckCollision(pipe)) //check collison of bird and pipe
                        {
                            _bird.SetIsAlive(false); //bird not alive if collide with bird
                            break; //exit loop if collision detect 
                        }
                    }
                }

                //draw game elements
                _window.Clear(Color.White);
                SplashKit.DrawBitmap(_gameBackgroundBitmap, 0, 0); // draw game background image
                _bird.Draw();
                foreach (Pipe pipe in _pipes)
                {
                    pipe.Draw();
                }
                SplashKit.DrawText("Score: " + _bird.GetScore(), Color.Black, 20, 20); //draw score of the flappybird game
                SplashKit.DrawText("High Score: " + _highScore, Color.Black, _window.Width - 150, 20);

                // display game over message and handle restart
                if (!_bird.IsAlive())
                {
                    SplashKit.DrawText("GAME OVER", Color.Red, 360, 250);
                    SplashKit.DrawText("Press R to restart", Color.Red, 330, 280);

                    // update high score if the current score is higher
                    if (_bird.GetScore() > _highScore)
                    {
                        _highScore = (int)_bird.GetScore(); //update the high score
                        SaveHighScore(_highScore); //save the high score to file
                    }

                    // restart the game if R key is pressed
                    if (SplashKit.KeyTyped(KeyCode.RKey))
                    {
                        _bird = new FlappyBird(); // create a new object of the bird
                        _pipes.Clear(); // clear list of pipes
                        _gameTimer.Reset();
                    }
                }

                _window.Refresh(60);
            }
        }

        // method to save the high score to a file
        private void SaveHighScore(int highScore)
        {
            File.WriteAllText("highscore.txt", highScore.ToString());
        }
    }

    public class HomePage
    {
        private Window _window;
        private int _highScore;
        private Bitmap _startButtonBitmap;
        private double _startButtonX;
        private double _startButtonY;
        private bool _startGame;
        private Bitmap _backgroundBitmap;

        public HomePage(Window window, int highScore)
        {
            _window = window;
            _highScore = highScore;
            _startButtonBitmap = SplashKit.LoadBitmap("startButton", "start4.png");
            _startButtonX = 310; //position of start button
            _startButtonY = 450; 
            _startGame = false; //game not start initially
            _backgroundBitmap = SplashKit.LoadBitmap("homeBackground", "bb.png");
        }
        

        //method to draw home page
        public void Draw()
        {
            _window.Clear(Color.White);
            SplashKit.DrawBitmap(_backgroundBitmap, 0, 0); // draw background image
            Bitmap logoBitmap = SplashKit.LoadBitmap("logo", "logo.png");
            SplashKit.DrawBitmap(logoBitmap, 130, 80); //draw logo
            SplashKit.DrawText("Instructions:", Color.Black, 150, 300); //display instruction

            // Split the instructions text into multiple lines
            string instructionsLine1 = "Press Space to make the bird flap and avoid";
            string instructionsLine2 = "hitting the pipes. Please try your best to exceed";
            string instructionsLine3 = "the highest score! Good Luck.";
            
            //draw instruction on the window screen
            SplashKit.DrawText(instructionsLine1, Color.Black, 200, 330);
            SplashKit.DrawText(instructionsLine2, Color.Black, 200, 360);
            SplashKit.DrawText(instructionsLine3, Color.Black, 200, 390);

            SplashKit.DrawText("Highest Score: " + _highScore, Color.Black, 340, 250); //draw the text (highest score) achieve by player on home page
            SplashKit.DrawBitmap(_startButtonBitmap, _startButtonX, _startButtonY); //draw start button (image) on the homepage
            _window.Refresh();
        }

        public void HandleInput()
        {
            SplashKit.ProcessEvents();

            if (SplashKit.MouseClicked(MouseButton.LeftButton)) //check if left mouse button is click
            {
                Point2D mousePosition = SplashKit.MousePosition();

                if (mousePosition.X >= _startButtonX && mousePosition.X <= _startButtonX + _startButtonBitmap.Width &&
                    mousePosition.Y >= _startButtonY && mousePosition.Y <= _startButtonY + _startButtonBitmap.Height)
                {
                    _startGame = true; //game will start if start button is click by left mouse button
                }
            }
        }

        public bool ShouldStartGame() //method to check if the game should start
        {
            return _startGame;
        }
    }

    public class Pipe
    {
        private double _x;
        private double _gapCenter;
        private double _gapHeight;
        private double _width;
        private bool _passed;
        private Bitmap _upperPipeBitmap;
        private Bitmap _lowerPipeBitmap;

        public Pipe(double x)
        {
            _x = x;
            _gapCenter = SplashKit.Rnd(200, 500); //randomize the pipe's gap center position
            _gapHeight = 160; //height of the gap between upperpipe and lowwerpipe
            _width = 80; //width of the pipe
            _passed = false; //pipe is not passed initially

            //load the pipe images
            _upperPipeBitmap = SplashKit.LoadBitmap("upperpipe", "upperpipe1.png");
            _lowerPipeBitmap = SplashKit.LoadBitmap("lowerpipe", "lowwerpipe1.png");
        }

        public void Draw()
        {
            double upperPipeHeight = _gapCenter - _gapHeight / 2; //calculate the vertical position of the upper pipe
            SplashKit.DrawBitmap(_upperPipeBitmap, _x, upperPipeHeight - _upperPipeBitmap.Height); //draw the upper pipe

            double lowerPipeY = _gapCenter + _gapHeight / 2; //calculate the vertical position of the lower pipe
            SplashKit.DrawBitmap(_lowerPipeBitmap, _x, lowerPipeY); //draw the lower pipe
        }

        public void Update() //speed of movement of pipe
        {
            _x -= 4;
        }

        public double X
        {
            get { return _x; }
        }

        public double GapCenter
        {
            get { return _gapCenter; }
        }

        public double GapHeight
        {
            get { return _gapHeight; }
        }

        public double Width
        {
            get { return _width; }
        }

        public bool Passed
        {
            get { return _passed; }
            set { _passed = value; }
        }
    }

    public class FlappyBird
    {
        //bird properties
        private Bitmap _birdBitmap;
        private double _birdX;
        private double _birdY;
        private double _birdSpeed;
        private double _score;
        private bool _isAlive;

        public FlappyBird()
        {
            _birdBitmap = SplashKit.LoadBitmap("bird", "bird.png");
            _birdX = 100;
            _birdY = 200;
            _birdSpeed = 0;
            _score = 0;
            _isAlive = true;
        }

        public void Draw()
        {
            SplashKit.DrawBitmap(_birdBitmap, _birdX, _birdY);
        }

        //method to update bird's position handle collision
        public void Update(List<Pipe> pipes)
        {
            if (_isAlive)
            {
                _birdSpeed += 0.5; //apply gravity effect
                _birdY += _birdSpeed; //update y-axis of bird's position

                if (SplashKit.KeyTyped(KeyCode.SpaceKey))
                {
                    _birdSpeed = -10; //apply upward force when flapped(space key is press)
                }

                if (_birdY < 0 || _birdY > 600)
                {
                    _isAlive = false; //bird is not alive if it is out of the screen
                }

                foreach (Pipe pipe in pipes) //check if the bird passes through a pipe
                {
                    if (_birdX > pipe.X + pipe.Width && !pipe.Passed)
                    {
                        IncreaseScore(); //increase score if passes a pipe
                        pipe.Passed = true; //mark pipe as passed to avoid duplicate score increment
                    }
                }
            }
        }

        public bool CheckCollision(Pipe pipe) //check collision with a pipe
        {
            //check if the bird is within the horizontal range of the pipe
            if (_birdX + _birdBitmap.Width > pipe.X && _birdX < pipe.X + pipe.Width)
            {
                //check if the bird is above the gap
                if (_birdY < pipe.GapCenter - pipe.GapHeight / 2)
                {
                    return true; //collision with the upper pipe
                }
                
                //check if the bird is below the gap
                if (_birdY + _birdBitmap.Height > pipe.GapCenter + pipe.GapHeight / 2)
                {
                    return true; //collision with the lower pipe
                }
            }
            
            return false; //no collision
        }

        public void IncreaseScore()
        {
            _score++;
        }

        public bool IsAlive()
        {
            return _isAlive;
        }

        public void SetIsAlive(bool isAlive)
        {
            _isAlive = isAlive;
        }

        public double GetScore()
        {
            return _score;
        }
    }
    
}