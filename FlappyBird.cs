using SplashKitSDK;
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
