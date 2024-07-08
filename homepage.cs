using SplashKitSDK;
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

   
    