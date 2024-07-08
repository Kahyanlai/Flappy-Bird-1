using SplashKitSDK;
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