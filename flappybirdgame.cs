using SplashKitSDK;
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
