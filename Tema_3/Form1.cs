using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tema_3
{
    public partial class GameWindow : Form
    {
        //Variables
        //Select random value from X, Y lists and assign a new location to each card
        Random CardLocation = new Random();
        List<Point> Points = new List<Point>(); // X, Y values for each picturebox
        List<Object> Tags = new List<Object>(); // store picture matches

        PictureBox PendingImage1; // Store first flipped card
        PictureBox PendingImage2; // Store second flipped card

        public GameWindow()
        {
            InitializeComponent();
        }

        private void CardsHolder_Paint(object sender, EventArgs e)
        {
            // Play again means I have to reinitialize these
            ScoreCounter.Text = "0";
            MoveCounter.Text = "0";
            start_time.Text = "1";

            foreach (PictureBox picture in CardsHolder.Controls)
            {
                picture.Enabled = false; // can't click pictures before they loaded
                Points.Add(picture.Location); // save picture locations on initialize
            }
            // Assign random location to each picture

            foreach (PictureBox picture in CardsHolder.Controls)
            {
                int next = CardLocation.Next(0, Points.Count); // generate random location
                Point p = Points[next]; // assign random location to a point
                picture.Location = p; // assign location to picture
                Points.Remove(p);  // make sure each location is unique
            }
            // 1 second reveal all cards, then cover them and allow click
            timer1.Start();
            timer2.Start();
            // Allocate image to each Card
            Card1.Image = Properties.Resources.Raspberries;
            Card1Dup.Image = Properties.Resources.Raspberries;
            Card2.Image = Properties.Resources.Avocado;
            Card2Dup.Image = Properties.Resources.Avocado;
            Card3.Image = Properties.Resources.Banana;
            Card3Dup.Image = Properties.Resources.Banana;
            Card4.Image = Properties.Resources.Cherries;
            Card4Dup.Image = Properties.Resources.Cherries;
            Card5.Image = Properties.Resources.Grapes;
            Card5Dup.Image = Properties.Resources.Grapes;
            Card6.Image = Properties.Resources.Grapefruit;
            Card6Dup.Image = Properties.Resources.Grapefruit;
            Card7.Image = Properties.Resources.Strawberry;
            Card7Dup.Image = Properties.Resources.Strawberry;
            Card8.Image = Properties.Resources.Plum;
            Card8Dup.Image = Properties.Resources.Plum;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            foreach (PictureBox picture in CardsHolder.Controls)
            {
                picture.Enabled = true;
                picture.Cursor = Cursors.Hand;
                picture.Image = Properties.Resources.Background;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            int timer = Convert.ToInt32(start_time.Text);
            timer--;
            start_time.Text = Convert.ToString(timer);
            if (timer == 0)
            {
                timer2.Stop();
                timer4.Start();
            }
        }
        // handles comparison between images, displays 2 images 1 second after click if not match
        // after you find a match, those pictures are disabled
        // else leaves the fruit images there
        private void timer3_Tick(object sender, EventArgs e)
        {
            timer3.Stop();
            PendingImage1.Image = Properties.Resources.Background;
            PendingImage2.Image = Properties.Resources.Background;
            PendingImage1 = null;
            PendingImage2 = null;
            foreach (PictureBox picture in CardsHolder.Controls)
            {
                if (Tags.Contains(picture.Tag))
                {
                    continue;
                }
                picture.Enabled = true;
            }
        }
        // How long they take to solve to solve the memory game
        private void timer4_Tick(object sender, EventArgs e)
        {
            int timer = Convert.ToInt32(start_time.Text);
            timer++;
            start_time.Text = Convert.ToString(timer);
        }
        // Play again button
        private void btnPlay_Click(object sender, EventArgs e)
        {
            timer4.Stop();
            CardsHolder_Paint(sender, e);
        }

        // Change Card Value
        // Handles most game logic. 
        #region Cards
        private void Card1_Click(object sender, EventArgs e)
        {
            // only comment first becaue it is the same for all. 
            // show image after click
            Card1.Image = Properties.Resources.Raspberries;
            // game logic
            // when no image is displayed
            if (PendingImage1 == null)
            {
                PendingImage1 = Card1;
            }
            // when 1 image is displayed
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                // deals with clicking on the same image twice
                if (PendingImage1 != Card1)
                    PendingImage2 = Card1;
            }
            // when I click on two images
            if(PendingImage1 != null && PendingImage2 != null)
            {
                // if match, I use Tag to compare match. 
                // My structure is CardX and CardXDup. I do comparisons between them.
                if(PendingImage1.Tag == PendingImage2.Tag)
                {
                    // disable click
                    Card1.Enabled = false;
                    Card1Dup.Enabled = false;
                    // reset logic
                    PendingImage1 = null;
                    PendingImage2 = null;
                    // save used tag
                    Tags.Add(Card1.Tag);
                    // update score
                    ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 10);
                    MoveCounter.Text = Convert.ToString(Convert.ToInt32(MoveCounter.Text) + 1);
                }
                // if no match
                else
                {
                    // update score
                    ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) - 10);
                    MoveCounter.Text = Convert.ToString(Convert.ToInt32(MoveCounter.Text) + 1);
                    // disable cards to not allow clicking multiple at once
                    // before the system can process
                    foreach (PictureBox picture in CardsHolder.Controls)
                    {
                        picture.Enabled = false;
                    }
                    // reset
                    timer3.Start();
                }
            }
        }

        private void Card1Dup_Click(object sender, EventArgs e)
        {
            Card1Dup.Image = Properties.Resources.Raspberries;
            if (PendingImage1 == null)
            {
                PendingImage1 = Card1Dup;
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                if (PendingImage1 != Card1Dup)
                    PendingImage2 = Card1Dup;
            }
            if (PendingImage1 != null && PendingImage2 != null)
            {
                if (PendingImage1.Tag == PendingImage2.Tag)
                {
                    Card1.Enabled = false;
                    Card1Dup.Enabled = false;
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Tags.Add(Card1Dup.Tag);
                    ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 10);
                    MoveCounter.Text = Convert.ToString(Convert.ToInt32(MoveCounter.Text) + 1);
                }
                else
                {
                    ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) - 10);
                    MoveCounter.Text = Convert.ToString(Convert.ToInt32(MoveCounter.Text) + 1);
                    foreach (PictureBox picture in CardsHolder.Controls)
                    {
                        picture.Enabled = false;
                    }
                    timer3.Start();
                }
            }
        }

        private void Card2_Click(object sender, EventArgs e)
        {
            Card2.Image = Properties.Resources.Avocado;
            if(PendingImage1 == null)
            {
                PendingImage1 = Card2;
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                if (PendingImage1 != Card2)
                    PendingImage2 = Card2;
            }
            if(PendingImage1 != null && PendingImage2 != null)
            {
                if(PendingImage1.Tag == PendingImage2.Tag)
                {
                    Card2.Enabled = false;
                    Card2Dup.Enabled = false;
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Tags.Add(Card2.Tag);
                    ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 10);
                    MoveCounter.Text = Convert.ToString(Convert.ToInt32(MoveCounter.Text) + 1);
                }
                else
                {
                    ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) - 10);
                    MoveCounter.Text = Convert.ToString(Convert.ToInt32(MoveCounter.Text) + 1);
                    foreach (PictureBox picture in CardsHolder.Controls)
                    {
                        picture.Enabled = false;
                    }
                    timer3.Start();
                }
            }
        }

        private void Card2Dup_Click(object sender, EventArgs e)
        {
            Card2Dup.Image = Properties.Resources.Avocado;
            if(PendingImage1 == null)
            {
                PendingImage1 = Card2Dup;
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                if (PendingImage1 != Card2Dup)
                    PendingImage2 = Card2Dup;
            }
            if(PendingImage1 != null && PendingImage2 != null)
            {
                if(PendingImage1.Tag == PendingImage2.Tag)
                {
                    Card2.Enabled = false;
                    Card2Dup.Enabled = false;
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Tags.Add(Card2Dup.Tag);
                    ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 10);
                    MoveCounter.Text = Convert.ToString(Convert.ToInt32(MoveCounter.Text) + 1);
                }
                else
                {
                    ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) - 10);
                    MoveCounter.Text = Convert.ToString(Convert.ToInt32(MoveCounter.Text) + 1);
                    foreach (PictureBox picture in CardsHolder.Controls)
                    {
                        picture.Enabled = false;
                    }
                    timer3.Start();
                }
             }
        }

        private void Card3_Click(object sender, EventArgs e)
        {
            Card3.Image = Properties.Resources.Banana;
            if (PendingImage1 == null)
            {
                PendingImage1 = Card3;
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                if (PendingImage1 != Card3)
                    PendingImage2 = Card3;
            }
            if (PendingImage1 != null && PendingImage2 != null)
            {
                if (PendingImage1.Tag == PendingImage2.Tag)
                {
                    Card3.Enabled = false;
                    Card3Dup.Enabled = false;
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Tags.Add(Card3.Tag);
                    ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 10);
                    MoveCounter.Text = Convert.ToString(Convert.ToInt32(MoveCounter.Text) + 1);
                }
                else
                {
                    ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) - 10);
                    MoveCounter.Text = Convert.ToString(Convert.ToInt32(MoveCounter.Text) + 1);
                    foreach (PictureBox picture in CardsHolder.Controls)
                    {
                        picture.Enabled = false;
                    }
                    timer3.Start();
                }
            }
        }

        private void Card3Dup_Click(object sender, EventArgs e)
        {
            Card3Dup.Image = Properties.Resources.Banana;
            if (PendingImage1 == null)
            {
                PendingImage1 = Card3Dup;
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                if (PendingImage1 != Card3Dup)
                    PendingImage2 = Card3Dup;
            }
            if (PendingImage1 != null && PendingImage2 != null)
            {
                if (PendingImage1.Tag == PendingImage2.Tag)
                {
                    Card3.Enabled = false;
                    Card3Dup.Enabled = false;
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Tags.Add(Card3Dup.Tag);
                    ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 10);
                    MoveCounter.Text = Convert.ToString(Convert.ToInt32(MoveCounter.Text) + 1);
                }
                else
                {
                    ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) - 10);
                    MoveCounter.Text = Convert.ToString(Convert.ToInt32(MoveCounter.Text) + 1);
                    foreach (PictureBox picture in CardsHolder.Controls)
                    {
                        picture.Enabled = false;
                    }
                    timer3.Start();
                }
            }
        }

        private void Card4_Click(object sender, EventArgs e)
        {
            Card4.Image = Properties.Resources.Cherries;
            if (PendingImage1 == null)
            {
                PendingImage1 = Card4;
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                if (PendingImage1 != Card4)
                    PendingImage2 = Card4;
            }
            if (PendingImage1 != null && PendingImage2 != null)
            {
                if (PendingImage1.Tag == PendingImage2.Tag)
                {
                    Card4.Enabled = false;
                    Card4Dup.Enabled = false;
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Tags.Add(Card4.Tag);
                    ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 10);
                    MoveCounter.Text = Convert.ToString(Convert.ToInt32(MoveCounter.Text) + 1);
                }
                else
                {
                    ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) - 10);
                    MoveCounter.Text = Convert.ToString(Convert.ToInt32(MoveCounter.Text) + 1);
                    foreach (PictureBox picture in CardsHolder.Controls)
                    {
                        picture.Enabled = false;
                    }
                    timer3.Start();
                }
            }
        }

        private void Card4Dup_Click(object sender, EventArgs e)
        {
            Card4Dup.Image = Properties.Resources.Cherries;
            if (PendingImage1 == null)
            {
                PendingImage1 = Card4Dup;
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                if (PendingImage1 != Card4Dup)
                    PendingImage2 = Card4Dup;
            }
            if (PendingImage1 != null && PendingImage2 != null)
            {
                if (PendingImage1.Tag == PendingImage2.Tag)
                {
                    Card4.Enabled = false;
                    Card4Dup.Enabled = false;
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Tags.Add(Card4Dup.Tag);
                    ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 10);
                    MoveCounter.Text = Convert.ToString(Convert.ToInt32(MoveCounter.Text) + 1);
                }
                else
                {
                    ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) - 10);
                    MoveCounter.Text = Convert.ToString(Convert.ToInt32(MoveCounter.Text) + 1);
                    foreach (PictureBox picture in CardsHolder.Controls)
                    {
                        picture.Enabled = false;
                    }
                    timer3.Start();
                }
            }
        }

        private void Card5_Click(object sender, EventArgs e)
        {
            Card5.Image = Properties.Resources.Grapes;
            if (PendingImage1 == null)
            {
                PendingImage1 = Card5;
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                if (PendingImage1 != Card5)
                    PendingImage2 = Card5;
            }
            if (PendingImage1 != null && PendingImage2 != null)
            {
                if (PendingImage1.Tag == PendingImage2.Tag)
                {
                    Card5.Enabled = false;
                    Card5Dup.Enabled = false;
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Tags.Add(Card5.Tag);
                    ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 10);
                    MoveCounter.Text = Convert.ToString(Convert.ToInt32(MoveCounter.Text) + 1);
                }
                else
                {
                    ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) - 10);
                    MoveCounter.Text = Convert.ToString(Convert.ToInt32(MoveCounter.Text) + 1);
                    foreach (PictureBox picture in CardsHolder.Controls)
                    {
                        picture.Enabled = false;
                    }
                    timer3.Start();
                }
            }
        }

        private void Card5Dup_Click(object sender, EventArgs e)
        {
            Card5Dup.Image = Properties.Resources.Grapes;
            if (PendingImage1 == null)
            {
                PendingImage1 = Card5Dup;
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                if (PendingImage1 != Card5Dup)
                    PendingImage2 = Card5Dup;
            }
            if (PendingImage1 != null && PendingImage2 != null)
            {
                if (PendingImage1.Tag == PendingImage2.Tag)
                {
                    Card5.Enabled = false;
                    Card5Dup.Enabled = false;
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Tags.Add(Card5Dup.Tag);
                    ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 10);
                    MoveCounter.Text = Convert.ToString(Convert.ToInt32(MoveCounter.Text) + 1);
                }
                else
                {
                    ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) - 10);
                    MoveCounter.Text = Convert.ToString(Convert.ToInt32(MoveCounter.Text) + 1);
                    foreach (PictureBox picture in CardsHolder.Controls)
                    {
                        picture.Enabled = false;
                    }
                    timer3.Start();
                }
            }
        }

        private void Card6_Click(object sender, EventArgs e)
        {
            Card6.Image = Properties.Resources.Grapefruit;
            if (PendingImage1 == null)
            {
                PendingImage1 = Card6;
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                if (PendingImage1 != Card6)
                    PendingImage2 = Card6;
            }
            if (PendingImage1 != null && PendingImage2 != null)
            {
                if (PendingImage1.Tag == PendingImage2.Tag)
                {
                    Card6.Enabled = false;
                    Card6Dup.Enabled = false;
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Tags.Add(Card6.Tag);
                    ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 10);
                    MoveCounter.Text = Convert.ToString(Convert.ToInt32(MoveCounter.Text) + 1);
                }
                else
                {
                    ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) - 10);
                    MoveCounter.Text = Convert.ToString(Convert.ToInt32(MoveCounter.Text) + 1);
                    foreach (PictureBox picture in CardsHolder.Controls)
                    {
                        picture.Enabled = false;
                    }
                    timer3.Start();
                }
            }
        }

        private void Card6Dup_Click(object sender, EventArgs e)
        {
            Card6Dup.Image = Properties.Resources.Grapefruit;
            if (PendingImage1 == null)
            {
                PendingImage1 = Card6Dup;
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                if (PendingImage1 != Card6Dup)
                    PendingImage2 = Card6Dup;
            }
            if (PendingImage1 != null && PendingImage2 != null)
            {
                if (PendingImage1.Tag == PendingImage2.Tag)
                {
                    Card6.Enabled = false;
                    Card6Dup.Enabled = false;
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Tags.Add(Card6Dup.Tag);
                    ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 10);
                    MoveCounter.Text = Convert.ToString(Convert.ToInt32(MoveCounter.Text) + 1);
                }
                else
                {
                    ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) - 10);
                    MoveCounter.Text = Convert.ToString(Convert.ToInt32(MoveCounter.Text) + 1);
                    foreach (PictureBox picture in CardsHolder.Controls)
                    {
                        picture.Enabled = false;
                    }
                    timer3.Start();
                }
            }
        }

        private void Card7_Click(object sender, EventArgs e)
        {
            Card7.Image = Properties.Resources.Strawberry;
            if (PendingImage1 == null)
            {
                PendingImage1 = Card7;
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                if (PendingImage1 != Card7)
                    PendingImage2 = Card7;
            }
            if (PendingImage1 != null && PendingImage2 != null)
            {
                if (PendingImage1.Tag == PendingImage2.Tag)
                {
                    Card7.Enabled = false;
                    Card7Dup.Enabled = false;
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Tags.Add(Card7.Tag);
                    ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 10);
                    MoveCounter.Text = Convert.ToString(Convert.ToInt32(MoveCounter.Text) + 1);
                }
                else
                {
                    ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) - 10);
                    MoveCounter.Text = Convert.ToString(Convert.ToInt32(MoveCounter.Text) + 1);
                    foreach (PictureBox picture in CardsHolder.Controls)
                    {
                        picture.Enabled = false;
                    }
                    timer3.Start();
                }
            }
        }

        private void Card7Dup_Click(object sender, EventArgs e)
        {
            Card7Dup.Image = Properties.Resources.Strawberry;
            if (PendingImage1 == null)
            {
                PendingImage1 = Card7Dup;
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                if (PendingImage1 != Card7Dup)
                    PendingImage2 = Card7Dup;
            }
            if (PendingImage1 != null && PendingImage2 != null)
            {
                if (PendingImage1.Tag == PendingImage2.Tag)
                {
                    Card7.Enabled = false;
                    Card7Dup.Enabled = false;
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Tags.Add(Card7Dup.Tag);
                    ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 10);
                    MoveCounter.Text = Convert.ToString(Convert.ToInt32(MoveCounter.Text) + 1);
                }
                else
                {
                    ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) - 10);
                    MoveCounter.Text = Convert.ToString(Convert.ToInt32(MoveCounter.Text) + 1);
                    foreach (PictureBox picture in CardsHolder.Controls)
                    {
                        picture.Enabled = false;
                    }
                    timer3.Start();
                }
            }
        }

        private void Card8_Click(object sender, EventArgs e)
        {
            Card8.Image = Properties.Resources.Plum;
            if (PendingImage1 == null)
            {
                PendingImage1 = Card8;
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                if (PendingImage1 != Card8)
                    PendingImage2 = Card8;
            }
            if (PendingImage1 != null && PendingImage2 != null)
            {
                if (PendingImage1.Tag == PendingImage2.Tag)
                {
                    Card8.Enabled = false;
                    Card8Dup.Enabled = false;
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Tags.Add(Card8.Tag);
                    ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 10);
                    MoveCounter.Text = Convert.ToString(Convert.ToInt32(MoveCounter.Text) + 1);
                }
                else
                {
                    ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) - 10);
                    MoveCounter.Text = Convert.ToString(Convert.ToInt32(MoveCounter.Text) + 1);
                    foreach (PictureBox picture in CardsHolder.Controls)
                    {
                        picture.Enabled = false;
                    }
                    timer3.Start();
                }
            }
        }

        private void Card8Dup_Click(object sender, EventArgs e)
        {
            Card8Dup.Image = Properties.Resources.Plum;
            if (PendingImage1 == null)
            {
                PendingImage1 = Card8Dup;
            }
            else if (PendingImage1 != null && PendingImage2 == null)
            {
                if (PendingImage1 != Card8Dup)
                    PendingImage2 = Card8Dup;
            }
            if (PendingImage1 != null && PendingImage2 != null)
            {
                if (PendingImage1.Tag == PendingImage2.Tag)
                {
                    Card8.Enabled = false;
                    Card8Dup.Enabled = false;
                    PendingImage1 = null;
                    PendingImage2 = null;
                    Tags.Add(Card8Dup.Tag);
                    ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) + 10);
                    MoveCounter.Text = Convert.ToString(Convert.ToInt32(MoveCounter.Text) + 1);
                }
                else
                {
                    ScoreCounter.Text = Convert.ToString(Convert.ToInt32(ScoreCounter.Text) - 10);
                    MoveCounter.Text = Convert.ToString(Convert.ToInt32(MoveCounter.Text) + 1);
                    foreach (PictureBox picture in CardsHolder.Controls)
                    {
                        picture.Enabled = false;
                    }
                    timer3.Start();
                }
            }
        }

        #endregion
    }
}
