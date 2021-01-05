using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace button_grid_test
{


    public partial class battleship : Form
    {
        System.Media.SoundPlayer bomb = new System.Media.SoundPlayer();//bomb sound - when a boat is sunken
        //System.Media.SoundPlayer BGM = new System.Media.SoundPlayer();//background music
        System.Media.SoundPlayer click = new System.Media.SoundPlayer();
        //initialize a background music player 



        public battleship()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            //plays background music automatically
            axWindowsMediaPlayer1.URL = @"BGM.wav";
            

            //initializa all the sound effects
            bomb.SoundLocation = @"bomb.wav";//initializa a sound
            click.SoundLocation = @"click.wav";//initializa a sound

            ////BGM.SoundLocation = @"BGM.wav";

            //play background music
            //BGM.PlayLooping();

            //make labels invisible
            label1.Visible = false;//player 1 name
            label2.Visible = false;//player 2 name
            label4.Visible = false;//

            pictureBox1.Visible = false;

            //clear every variable
            Globals.ship1.Clear();
            Globals.ship2.Clear();
            Globals.chosen1.Clear();
            Globals.chosen2.Clear();
            Globals.shipchoose = 1;
            Globals.chooseturn = 1;
            Globals.turn = 1;
            Globals.hit1 = true;
            Globals.hit2 = true;
            //the intro page
            pictureBox2.Visible = true;
            title.Visible = true;
            button1.Visible = true;
            button2.Visible = true;
            //if it;s not the first turn then reset all the boards
            if (Globals.initial == false)
            {
                for (int x = 0; x < 10; x++)
                {
                    for (int y = 0; y < 10; y++)
                    {
                        Globals.empty1[x][y].Visible = false;
                        Globals.empty2[x][y].Visible = false;
                        Globals.cover1[x][y].Visible = false;
                        Globals.cover2[x][y].Visible = false;
                        Globals.visible1[x][y].Visible = false;
                        Globals.visible2[x][y].Visible = false;
                    }
                }
            }
            Globals.initial = false;
            //initialization
            Globals.empty1.Clear();
            Globals.empty2.Clear();
            Globals.cover1.Clear();
            Globals.cover2.Clear();
            Globals.visible1.Clear();
            Globals.visible2.Clear();
           
            Globals.robot = false;//checks if player chooses to enter robot mode
            Globals.row2 = 0;
            Globals.column2 = 0;
            Globals.decide = 0;
            Globals.shipnum = 0;
            Globals.player1 = true;//resets the value
            Globals.win1 = true;
            Globals.win2 = true;

            for (int x = 0; x < 10; x++)
            {
                Globals.ship1.Add(new List<int>());//stores player 1's ship placements as integers
                Globals.ship2.Add(new List<int>());//stores player 2's ship placements as integers
                for (int y = 0; y < 10; y++)
                {
                    Globals.ship1[x].Add(0);
                    Globals.ship2[x].Add(0);
                }
            }


            List<List<Button>> b1 = new List<List<Button>>();//buttons for the chooseing part

            foreach (int i in Enumerable.Range(0, 10))
            {
                b1.Add(new List<Button>());

                foreach (int j in Enumerable.Range(0, 10))
                {
                    b1[i].Add(new Button());

                    b1[i][j].Size = new System.Drawing.Size(80, 80);
                    b1[i][j].Location = new Point(i * 80, 50 + j * 80);
                    b1[i][j].BackColor = Color.Blue;
                    b1[i][j].Click += (senderA, A) => returnposition(senderA, A, b1, i, j);
                    this.Controls.Add(b1[i][j]);
                }
            }

            label3.Visible = true;//player 1 intro
            label5.Visible = false;//player 1 intro

        }

        void player2choose(object senderP2, EventArgs P2, Button Next)//choosing process for player 2
        {
            Next.Visible = false;
            List<List<Button>> b2 = new List<List<Button>>();//choosing board
            foreach (int i in Enumerable.Range(0, 10))
            {
                b2.Add(new List<Button>());

                foreach (int j in Enumerable.Range(0, 10))
                {
                    b2[i].Add(new Button());
                    b2[i][j].BringToFront();
                    b2[i][j].Size = new System.Drawing.Size(80, 80);
                    b2[i][j].Location = new Point(i * 80, 50 + j * 80);
                    b2[i][j].BackColor = Color.Blue;
                    b2[i][j].Click += (senderA, A) => returnposition(senderA, A, b2, i, j);
                    this.Controls.Add(b2[i][j]);
                }
            }

            label3.Visible = false;//player 1 intro
            label5.Visible = true;//player 1 intro


        }

        void returnposition(object senderA, EventArgs A, List<List<Button>> b, int i, int j)
        {
            //chooseturn stores if its player 1 or 2's turn
            click.Play();

            var button = (senderA as Button);//object sender

            if (Globals.chooseturn == 1)//player 1's turn
            {
                for (int x = 0; x < 10; x++) for (int y = 0; y < 10; y++) if (Globals.ship1[x][y] == 0) b[x][y].Image = Image.FromFile(@"Selection\Clear.png");
                if (Globals.ship1[i][j] == 0) b[i][j].Image = Image.FromFile(@"Selection\Selected.png");
            }
            else if (Globals.chooseturn == 2)//player 2's turn
            {
                for (int x = 0; x < 10; x++) for (int y = 0; y < 10; y++) if (Globals.ship2[x][y] == 0) b[x][y].Image = Image.FromFile(@"Selection\Clear.png");
                if (Globals.ship2[i][j] == 0) b[i][j].Image = Image.FromFile(@"Selection\Selected.png");
            }

            Button up = new Button();//choosing directions
            Button down = new Button();
            Button left = new Button();
            Button right = new Button();

            int shiplength = 0;
            if (Globals.shipchoose == 1) shiplength = 5;//assign ship length to a variable
            else if (Globals.shipchoose == 2) shiplength = 4;
            else if (Globals.shipchoose == 3) shiplength = 3;
            else if (Globals.shipchoose == 4) shiplength = 3;
            else if (Globals.shipchoose == 5) shiplength = 2;
            bool upcheck = true;
            for (int x = 0; x < shiplength; x++)
            {
                if (Globals.chooseturn == 1)
                {
                    if (j - x < 0) upcheck = false;
                    else if (Globals.ship1[i][j - x] != 0) upcheck = false;
                }
                if (Globals.chooseturn == 2)
                {
                    if (j - x < 0) upcheck = false;
                    else if (Globals.ship2[i][j - x] != 0) upcheck = false;
                }
            }
            if (upcheck == true)//check if the boat can be placed upwards
            {
                up.Size = new System.Drawing.Size(80, 80);
                up.Location = new Point(1000, 450);
                up.Image = Image.FromFile(@"Selection\Up.png");
                up.Click += new EventHandler((senderB, B) => boatup(senderA, A, b, up, down, left, right, shiplength, i, j));// <-- all wired to the same handler
                this.Controls.Add(up);
            }
            bool downcheck = true;//checks if the boat can be placed down
            for (int x = 0; x < shiplength; x++)
            {
                if (Globals.chooseturn == 1)
                {
                    if (j + x > 9) downcheck = false;
                    else if (Globals.ship1[i][j + x] != 0) downcheck = false;
                }
                if (Globals.chooseturn == 2)
                {
                    if (j + x > 9) downcheck = false;
                    else if (Globals.ship2[i][j + x] != 0) downcheck = false;
                }
            }
            if (downcheck == true)
            {
                down.Size = new System.Drawing.Size(80, 80);
                down.Location = new Point(1000, 610);
                down.Image = Image.FromFile(@"Selection\Down.png");
                down.Click += new EventHandler((senderB, B) => boatdown(senderA, A, b, up, down, left, right, shiplength, i, j));// <-- all wired to the same handler
                this.Controls.Add(down);
            }
            bool leftcheck = true;// checks if the boat can be placed down
            for (int x = 0; x < shiplength; x++)
            {
                if (Globals.chooseturn == 1)
                {
                    if (i - x < 0) leftcheck = false;
                    else if (Globals.ship1[i - x][j] != 0) leftcheck = false;
                }
                if (Globals.chooseturn == 2)
                {
                    if (i - x < 0) leftcheck = false;
                    else if (Globals.ship2[i - x][j] != 0) leftcheck = false;
                }
            }
            if (leftcheck == true)
            {
                left.Size = new System.Drawing.Size(80, 80);
                left.Location = new Point(920, 530);
                left.Image = Image.FromFile(@"Selection\Left.png");
                left.Click += new EventHandler((senderB, B) => boatleft(senderA, A, b, up, down, left, right, shiplength, i, j));// <-- all wired to the same handler
                this.Controls.Add(left);
            }
            bool rightcheck = true;
            for (int x = 0; x < shiplength; x++)
            {
                if (Globals.chooseturn == 1)
                {
                    if (i + x > 9) rightcheck = false;
                    else if (Globals.ship1[i + x][j] != 0) rightcheck = false;
                }
                if (Globals.chooseturn == 2)
                {
                    if (i + x > 9) rightcheck = false;
                    else if (Globals.ship2[i + x][j] != 0) rightcheck = false;
                }
            }
            if (rightcheck == true)// checks if the boat can be placed down
            {
                right.Size = new System.Drawing.Size(80, 80);
                right.Location = new Point(1080, 530);
                right.Image = Image.FromFile(@"Selection\Right.png");
                right.Click += new EventHandler((senderB, B) => boatright(senderA, A, b, up, down, left, right, shiplength, i, j));// <-- all wired to the same handler
                this.Controls.Add(right);
            }

            if (Globals.shipchoose == 6 && Globals.chooseturn == 1)//end the choosing process
            {
                for (int x = 0; x < 10; x++) for (int y = 0; y < 10; y++) b[x][y].Visible = false;
                up.Visible = false;
                down.Visible = false;
                left.Visible = false;
                right.Visible = false;
                Globals.shipchoose = 1;
                if (Globals.robot == false)//if in friend vs friend mode
                {
                    Globals.chooseturn = 2;

                    Button Next = new Button();
                    Next.Size = new System.Drawing.Size(1200, 300);
                    Next.Location = new Point(165, 310);
                    Next.Image = Image.FromFile(@"continue.png");

                    Next.Click += new EventHandler((senderB, B) => player2choose(senderA, A, Next));
                    this.Controls.Add(Next);
                }
                else if (Globals.robot == true)//goes to robot mode
                {
                    Button Next = new Button();
                    Next.Size = new System.Drawing.Size(1200, 300);
                    Next.Location = new Point(165, 310);
                    Next.Image = Image.FromFile(@"continue.png");
                    robotselect();
                    Next.Click += new EventHandler((senderB, B) => Game(senderA, A, Next));//exits player 2's part
                    this.Controls.Add(Next);
                    label3.Visible = false;

                }
            }
            else if (Globals.shipchoose == 6 && Globals.chooseturn == 2)
            {
                for (int x = 0; x < 10; x++) for (int y = 0; y < 10; y++) b[x][y].Visible = false;
                up.Visible = false;
                down.Visible = false;
                left.Visible = false;
                right.Visible = false;
                Button Next = new Button();//the continue button
                Next.Size = new System.Drawing.Size(1200, 300);
                Next.Location = new Point(165, 310);
                Next.Image = Image.FromFile(@"continue.png");
                Next.Click += new EventHandler((senderB, B) => Game(senderA, A, Next));
                this.Controls.Add(Next);
            }
            foreach (int x in Enumerable.Range(0, 10))
            {

                foreach (int y in Enumerable.Range(0, 10))
                {
                    b[x][y].Click += (senderB, B) => reselectclear(senderB, B, up, down, left, right);//allows reselecting
                }
            }
        }

        void reselectclear(object senderB, EventArgs B, Button up, Button down, Button left, Button right)//removes arrrows for previously selected positions
        {
            
            up.Visible = false;
            down.Visible = false;
            left.Visible = false;
            right.Visible = false;
        }

        void boatup(object senderB, EventArgs B, List<List<Button>> b, Button up, Button down, Button left, Button right, int shiplength, int i, int j)//activates when the up button is pressed
        {
            up.Visible = false;
            down.Visible = false;
            left.Visible = false;
            right.Visible = false;
            Random rand = new Random();
            var button = (senderB as Button);
            for (int x = 0; x < shiplength; x++)
            {
                if (Globals.chooseturn == 1) Globals.ship1[i][j - x] = Globals.shipchoose;
                else if (Globals.chooseturn == 2) Globals.ship2[i][j - x] = Globals.shipchoose;
                if (x == 0) b[i][j].Image = Image.FromFile(@"ShipVert\bottom.png");
                else if (x == shiplength - 1) b[i][j - x].Image = Image.FromFile(@"ShipVert\top.png");
                else
                {
                    int part = rand.Next(0, 3);
                    if (part == 0) b[i][j - x].Image = Image.FromFile(@"ShipVert\middle no cannon.png");
                    else if (part == 1) b[i][j - x].Image = Image.FromFile(@"ShipVert\middle cannon up.png");
                    else if (part == 2) b[i][j - x].Image = Image.FromFile(@"ShipVert\middle cannon down.png");
                }
            }
            Globals.shipchoose += 1;
            if (Globals.shipchoose == 6) returnposition(senderB, B, b, i, j);
        }

        void boatdown(object senderB, EventArgs B, List<List<Button>> b, Button up, Button down, Button left, Button right, int shiplength, int i, int j)//activates when the down button is pressed
        {
            up.Visible = false;
            down.Visible = false;
            left.Visible = false;
            right.Visible = false;
            Random rand = new Random();
            var button = (senderB as Button);
            for (int x = 0; x < shiplength; x++)
            {
                if (Globals.chooseturn == 1) Globals.ship1[i][j + x] = Globals.shipchoose;
                else if (Globals.chooseturn == 2) Globals.ship2[i][j + x] = Globals.shipchoose;
                if (x == 0) b[i][j].Image = Image.FromFile(@"ShipVert\top.png");
                else if (x == shiplength - 1) b[i][j + x].Image = Image.FromFile(@"ShipVert\bottom.png");
                else
                {
                    int part = rand.Next(0, 3);
                    if (part == 0) b[i][j + x].Image = Image.FromFile(@"ShipVert\middle no cannon.png");
                    else if (part == 1) b[i][j + x].Image = Image.FromFile(@"ShipVert\middle cannon up.png");
                    else if (part == 2) b[i][j + x].Image = Image.FromFile(@"ShipVert\middle cannon down.png");
                }
            }
            Globals.shipchoose += 1;
            if (Globals.shipchoose == 6) returnposition(senderB, B, b, i, j);
        }

        void boatleft(object senderB, EventArgs B, List<List<Button>> b, Button up, Button down, Button left, Button right, int shiplength, int i, int j)
        {
            up.Visible = false;
            down.Visible = false;
            left.Visible = false;
            right.Visible = false;
            Random rand = new Random();
            var button = (senderB as Button);
            for (int x = 0; x < shiplength; x++)
            {
                if (Globals.chooseturn == 1) Globals.ship1[i - x][j] = Globals.shipchoose;
                else if (Globals.chooseturn == 2) Globals.ship2[i - x][j] = Globals.shipchoose;
                if (x == 0) b[i][j].Image = Image.FromFile(@"ShipHori\right.png");
                else if (x == shiplength - 1) b[i - x][j].Image = Image.FromFile(@"ShipHori\left.png");
                else
                {
                    int part = rand.Next(0, 3);
                    if (part == 0) b[i - x][j].Image = Image.FromFile(@"ShipHori\middle no cannon.png");
                    else if (part == 1) b[i - x][j].Image = Image.FromFile(@"ShipHori\middle cannon left.png");
                    else if (part == 2) b[i - x][j].Image = Image.FromFile(@"ShipHori\middle cannon right.png");
                }
            }
            Globals.shipchoose += 1;
            if (Globals.shipchoose == 6) returnposition(senderB, B, b, i, j);
        }

        void boatright(object senderB, EventArgs B, List<List<Button>> b, Button up, Button down, Button left, Button right, int shiplength, int i, int j)
        {
            up.Visible = false;
            down.Visible = false;
            left.Visible = false;
            right.Visible = false;
            Random rand = new Random();
            var button = (senderB as Button);
            for (int x = 0; x < shiplength; x++)
            {
                if (Globals.chooseturn == 1) Globals.ship1[i + x][j] = Globals.shipchoose;
                else if (Globals.chooseturn == 2) Globals.ship2[i + x][j] = Globals.shipchoose;
                if (x == 0) b[i][j].Image = Image.FromFile(@"ShipHori\left.png");
                else if (x == shiplength - 1) b[i + x][j].Image = Image.FromFile(@"ShipHori\right.png");
                else
                {
                    int part = rand.Next(0, 3);
                    if (part == 0) b[i + x][j].Image = Image.FromFile(@"ShipHori\middle no cannon.png");
                    else if (part == 1) b[i + x][j].Image = Image.FromFile(@"ShipHori\middle cannon left.png");
                    else if (part == 2) b[i + x][j].Image = Image.FromFile(@"ShipHori\middle cannon right.png");
                }
            }
            Globals.shipchoose += 1;
            if (Globals.shipchoose == 6) returnposition(senderB, B, b, i, j);
        }

        //game process
        void Game(object senderB, EventArgs B, Button Next)
        {
            Next.Visible = false;

            label5.Visible = false;//player 1 intro

            label1.Visible = true;
            label2.Visible = true;
            label4.Visible = true;


            foreach (int i in Enumerable.Range(0, 10))
            {
                Globals.visible1.Add(new List<Button>());//blocks the player's own boards so he cant click
                Globals.visible2.Add(new List<Button>());//blocks the player's own boards so he cant click

                foreach (int j in Enumerable.Range(0, 10))
                {
                    //make the cover button
                    Globals.visible1[i].Add(new Button());
                    Globals.visible1[i][j].Size = new System.Drawing.Size(60, 60);
                    Globals.visible1[i][j].Location = new Point(i * 60, 150 + j * 60);
                    Globals.visible1[i][j].BackColor = Color.Pink;
                    this.Controls.Add(Globals.visible1[i][j]);

                    Globals.visible2[i].Add(new Button());
                    Globals.visible2[i][j].Size = new System.Drawing.Size(60, 60);
                    Globals.visible2[i][j].Location = new Point(800 + i * 60, 150 + j * 60);
                    Globals.visible2[i][j].BackColor = Color.Pink;
                    this.Controls.Add(Globals.visible2[i][j]);
                }
            }

            foreach (int i in Enumerable.Range(0, 10))
            {
                Globals.cover1.Add(new List<Button>());
                Globals.cover2.Add(new List<Button>());

                foreach (int j in Enumerable.Range(0, 10))
                {
                    //make 
                    Globals.cover1[i].Add(new Button());
                    Globals.cover1[i][j].Size = new System.Drawing.Size(60, 60);
                    Globals.cover1[i][j].Location = new Point(i * 60, 150 + j * 60);
                    if (Globals.robot == false)
                    {
                        Globals.cover1[i][j].Click += (senderA, A) => xiu(senderA, A, i, j);
                    }

                    this.Controls.Add(Globals.cover1[i][j]);

                    Globals.cover1[i][j].BackColor = Color.Pink;


                    Globals.cover2[i].Add(new Button());
                    Globals.cover2[i][j].Size = new System.Drawing.Size(60, 60);
                    Globals.cover2[i][j].Location = new Point(800 + i * 60, 150 + j * 60);

                    Globals.cover2[i][j].Click += (senderA, A) => xiu2(senderA, A, i, j);
                    this.Controls.Add(Globals.cover2[i][j]);

                    Globals.cover2[i][j].BackColor = Color.Pink;

                }
            }

            for (int x = 0; x < 10; x++)
            {
                Globals.chosen1.Add(new List<bool>());
                Globals.chosen2.Add(new List<bool>());
                Globals.empty1.Add(new List<Button>());
                Globals.empty2.Add(new List<Button>());
                for (int y = 0; y < 10; y++)
                {
                    Globals.chosen1[x].Add(false);
                    Globals.chosen2[x].Add(false);
                    Globals.empty1[x].Add(new Button());
                    Globals.empty1[x][y].Size = new System.Drawing.Size(60, 60);
                    Globals.empty1[x][y].Location = new Point(x * 60, 150 + y * 60);

                    this.Controls.Add(Globals.empty1[x][y]);

                    if (Globals.ship1[x][y] == 0) Globals.empty1[x][y].BackColor = Color.Blue;
                    else Globals.empty1[x][y].BackColor = Color.Red;



                    Globals.empty2[x].Add(new Button());
                    Globals.empty2[x][y].Size = new System.Drawing.Size(60, 60);
                    Globals.empty2[x][y].Location = new Point(800 + x * 60, 150 + y * 60);

                    this.Controls.Add(Globals.empty2[x][y]);

                    if (Globals.ship2[x][y] == 0) Globals.empty2[x][y].BackColor = Color.Blue;
                    else Globals.empty2[x][y].BackColor = Color.Red;
                }
            }

            //List<List<Button>> visible1 = new List<List<Button>>();//blocks player 2's board - an invisible layer on top so a player cannot click on his own board
            //List<List<Button>> visible2 = new List<List<Button>>();//blocls player 1's board

            for (int z = 0; z < 10; z++) for (int t = 0; t < 10; t++) Globals.visible2[z][t].Visible = false;

            label1.BackColor = System.Drawing.Color.Transparent;
            label2.BackColor = System.Drawing.Color.Red;


        }

        //robot selecting part
        void robotselect()
        {
            List<int> shipsize = new List<int>() { 5, 4, 3, 3, 2 };
            int shipnum = 1;
            int row = 0;
            int column = 0;
            int direction;
            int increase;
            int incorrect;
            int select = 0;

            Random rand = new Random();

            //for (int a = 0; a < 10; a++) //initialize list
            //{
            //    Globals.ship2.Add(new List<int>());
            //    for (int b = 0; b < 10; b++)
            //    {
            //        Globals.ship2[a].Add(0);
            //    }
            //}

            for (int a = 0; a < 5; a++) //place 5 ships
            {
                do //loop until ship has been selected
                {
                    incorrect = 0;
                    select = 0;
                    row = rand.Next(0, 10); //random coordinates
                    column = rand.Next(0, 10);
                    direction = rand.Next(1, 5); //1 = up, 2 = right, 3 = down, 4 = left
                    if (direction == 1) //up
                    {
                        if (row - shipsize[a] < -1) //check if hits wall
                        {
                            continue;
                        }
                        for (increase = 0; increase < shipsize[a]; increase++) //check if all spots empty
                        {
                            if (Globals.ship2[row - increase][column] != 0)
                            {
                                incorrect = 1;
                                break;
                            }
                        }
                        if (incorrect == 0) //if all spots empty, place ship there
                        {
                            for (increase = 0; increase < shipsize[a]; increase++)
                            {
                                Globals.ship2[row - increase][column] = shipnum;
                            }
                            shipnum++;
                            select = 1;
                        }
                    }
                    else if (direction == 2) //right
                    {
                        if (column + shipsize[a] > 10) //check if hits wall
                        {
                            continue;
                        }
                        for (increase = 0; increase < shipsize[a]; increase++) //check if all spots empty
                        {
                            if (Globals.ship2[row][column + increase] != 0)
                            {
                                incorrect = 1;
                                break;
                            }
                        }
                        if (incorrect == 0) //if all spots empty, place ship there
                        {
                            for (increase = 0; increase < shipsize[a]; increase++)
                            {
                                Globals.ship2[row][column + increase] = shipnum;
                            }
                            shipnum++;
                            select = 1;
                        }
                    }
                    else if (direction == 3) //down
                    {
                        if (row + shipsize[a] > 10) //check if hits wall
                        {
                            continue;
                        }
                        for (increase = 0; increase < shipsize[a]; increase++) //check if all spots empty
                        {
                            if (Globals.ship2[row + increase][column] != 0)
                            {
                                incorrect = 1;
                                break;
                            }
                        }
                        if (incorrect == 0) //if all spots empty, place ship there
                        {
                            for (increase = 0; increase < shipsize[a]; increase++)
                            {
                                Globals.ship2[row + increase][column] = shipnum;
                            }
                            shipnum++;
                            select = 1;
                        }
                    }
                    else //left
                    {
                        if (column - shipsize[a] < -1) //check if hits wall
                        {
                            continue;
                        }
                        for (increase = 0; increase < shipsize[a]; increase++) //check if all spots empty
                        {
                            if (Globals.ship2[row][column - increase] != 0)
                            {
                                incorrect = 1;
                                break;
                            }
                        }
                        if (incorrect == 0) //if all spots empty, place ship there
                        {
                            for (increase = 0; increase < shipsize[a]; increase++)
                            {
                                Globals.ship2[row][column - increase] = shipnum;
                            }
                            shipnum++;
                            select = 1;
                        }
                    }
                } while (select == 0);
            }
        }



        //player 1's selecting process- the player
        //robot part in the robot mode
        void xiu(Object sender, EventArgs e, int i, int j)
        {
            //System.Threading.Thread.Sleep(2000);//dramatic effect :)
            //can't do lag because we display the pictures of boats at once. 
            //if (Globals.robot == true)
            //{
            //    System.Threading.Thread.Sleep(1000);
            //    for (int z = 0; z < 10; z++) for (int t = 0; t < 10; t++) Globals.visible1[z][t].Visible = false;
            //    for (int z = 0; z < 10; z++) for (int t = 0; t < 10; t++) if (Globals.chosen1[z][t] == false) Globals.visible1[z][t].Visible = true;
            //}
            
            click.Play();
            
            var button = (sender as Button);
            button.Visible = false;
            Globals.chosen1[i][j] = true;
            //robot part
            if (Globals.robot == true)
            {
                Globals.cover1[i][j].Visible = false;
                //MessageBox.Show("becomes visible");
            }

            Globals.win2 = true;

            if (Globals.ship1[i][j] == 0) Globals.hit2 = false;//check if one click on the ship or not
            else Globals.hit2 = true;
            bool sunken = true;//stores if a boat is sunken

            switch (Globals.ship1[i][j])//checks is a boat is sunken
            {
                case 1:
                    for (int x = 0; x < 10; x++) for (int y = 0; y < 10; y++) if (Globals.ship1[x][y] == 1 && Globals.chosen1[x][y] == false) sunken = false;
                    if (sunken == true) for (int x = 0; x < 10; x++) for (int y = 0; y < 10; y++) if (Globals.ship1[x][y] == 1)
                                {
                                    Globals.empty1[x][y].Image = Image.FromFile(@"sunken1.png");
                                    bomb.Play();
                                }
                    break;
                case 2:
                    for (int x = 0; x < 10; x++) for (int y = 0; y < 10; y++) if (Globals.ship1[x][y] == 2 && Globals.chosen1[x][y] == false) sunken = false;
                    if (sunken == true) for (int x = 0; x < 10; x++) for (int y = 0; y < 10; y++) if (Globals.ship1[x][y] == 2)
                                {
                                    Globals.empty1[x][y].Image = Image.FromFile(@"sunken2.png");
                                    bomb.Play();
                                }
                    break;
                case 3:
                    for (int x = 0; x < 10; x++) for (int y = 0; y < 10; y++) if (Globals.ship1[x][y] == 3 && Globals.chosen1[x][y] == false) sunken = false;
                    if (sunken == true) for (int x = 0; x < 10; x++) for (int y = 0; y < 10; y++) if (Globals.ship1[x][y] == 3)
                                {
                                    Globals.empty1[x][y].Image = Image.FromFile(@"sunken3.png");
                                    bomb.Play();
                                }
                    break;
                case 4:
                    for (int x = 0; x < 10; x++) for (int y = 0; y < 10; y++) if (Globals.ship1[x][y] == 4 && Globals.chosen1[x][y] == false) sunken = false;
                    if (sunken == true) for (int x = 0; x < 10; x++) for (int y = 0; y < 10; y++) if (Globals.ship1[x][y] == 4)
                                {
                                    Globals.empty1[x][y].Image = Image.FromFile(@"sunken4.png");
                                    bomb.Play();
                                }
                    break;
                case 5:
                    for (int x = 0; x < 10; x++) for (int y = 0; y < 10; y++) if (Globals.ship1[x][y] == 5 && Globals.chosen1[x][y] == false) sunken = false;
                    if (sunken == true) for (int x = 0; x < 10; x++) for (int y = 0; y < 10; y++) if (Globals.ship1[x][y] == 5)
                                {
                                    Globals.empty1[x][y].Image = Image.FromFile(@"sunken5.png");
                                    bomb.Play();
                                }
                    break;
            }


            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    if (Globals.ship1[x][y] > 0 && Globals.chosen1[x][y] == false) Globals.win2 = false;
                }
            }
            if (Globals.win2 == true)
            {
                for (int z = 0; z < 10; z++) for (int t = 0; t < 10; t++) Globals.visible1[z][t].Visible = false;
                for (int z = 0; z < 10; z++) for (int t = 0; t < 10; t++) if (Globals.chosen1[z][t] == false) Globals.visible1[z][t].Visible = true;
                label4.Visible = false;
                
                if (Globals.robot == false)
                {
                    pictureBox1.Visible = true;
                    MessageBox.Show("Player 2 has won the game!");
                }
                else
                {
                    MessageBox.Show("Oops, you lost to the robot ;(");
                }
                Form1_Load(sender, e);
            }

            //string h = Globals.hit2.ToString();
            //MessageBox.Show(h);
            if (Globals.win2 == false && Globals.robot == true)
            {
                for (int z = 0; z < 10; z++) for (int t = 0; t < 10; t++) Globals.visible1[z][t].Visible = false;
                for (int z = 0; z < 10; z++) for (int t = 0; t < 10; t++) if (Globals.chosen1[z][t] == false) Globals.visible1[z][t].Visible = true;
            }

            if (Globals.hit2 == false )//loops this one during the game
            {
                for (int z = 0; z < 10; z++) for (int t = 0; t < 10; t++) if (Globals.chosen1[z][t] == false) Globals.visible1[z][t].Visible = true;
                for (int z = 0; z < 10; z++) for (int t = 0; t < 10; t++) Globals.visible2[z][t].Visible = false;
                label1.BackColor = System.Drawing.Color.Transparent;
                label2.BackColor = System.Drawing.Color.Red;
                //System.Threading.Thread.Sleep(1000);
            }

            
        }

        //player 1's selecting process, this is also the part for player in the robot mode
        void xiu2(Object sender, EventArgs e, int i, int j)
        {
            if (Globals.player1 == true)
            {
                click.Play();
                var button = (sender as Button);
                button.Visible = false;
                Globals.chosen2[i][j] = true;
                Globals.win1 = true;
                Globals.player1 = true;

                if (Globals.ship2[i][j] == 0) Globals.hit1 = false;//check if one click on the ship or not
                else Globals.hit1 = true;

                bool sunken = true;


                switch (Globals.ship2[i][j])//checks is a boat is sunken
                {
                    case 1:
                        for (int x = 0; x < 10; x++) for (int y = 0; y < 10; y++) if (Globals.ship2[x][y] == 1 && Globals.chosen2[x][y] == false) sunken = false;
                        if (sunken == true) for (int x = 0; x < 10; x++) for (int y = 0; y < 10; y++) if (Globals.ship2[x][y] == 1)
                                    {
                                        Globals.empty2[x][y].Image = Image.FromFile(@"sunken1.png");
                                        bomb.Play();
                                    }
                        break;
                    case 2:
                        for (int x = 0; x < 10; x++) for (int y = 0; y < 10; y++) if (Globals.ship2[x][y] == 2 && Globals.chosen2[x][y] == false) sunken = false;
                        if (sunken == true) for (int x = 0; x < 10; x++) for (int y = 0; y < 10; y++) if (Globals.ship2[x][y] == 2)
                                    {
                                        Globals.empty2[x][y].Image = Image.FromFile(@"sunken2.png");
                                        bomb.Play();
                                    }
                        break;
                    case 3:
                        for (int x = 0; x < 10; x++) for (int y = 0; y < 10; y++) if (Globals.ship2[x][y] == 3 && Globals.chosen2[x][y] == false) sunken = false;
                        if (sunken == true) for (int x = 0; x < 10; x++) for (int y = 0; y < 10; y++) if (Globals.ship2[x][y] == 3)
                                    {
                                        Globals.empty2[x][y].Image = Image.FromFile(@"sunken3.png");
                                        bomb.Play();
                                    }
                        break;
                    case 4:
                        for (int x = 0; x < 10; x++) for (int y = 0; y < 10; y++) if (Globals.ship2[x][y] == 4 && Globals.chosen2[x][y] == false) sunken = false;
                        if (sunken == true) for (int x = 0; x < 10; x++) for (int y = 0; y < 10; y++) if (Globals.ship2[x][y] == 4)
                                    {
                                        Globals.empty2[x][y].Image = Image.FromFile(@"sunken4.png");
                                        bomb.Play();
                                    }
                        break;
                    case 5:
                        for (int x = 0; x < 10; x++) for (int y = 0; y < 10; y++) if (Globals.ship2[x][y] == 5 && Globals.chosen2[x][y] == false) sunken = false;
                        if (sunken == true) for (int x = 0; x < 10; x++) for (int y = 0; y < 10; y++) if (Globals.ship2[x][y] == 5)
                                    {
                                        Globals.empty2[x][y].Image = Image.FromFile(@"sunken5.png");
                                        bomb.Play();
                                    }
                        break;
                }



                for (int x = 0; x < 10; x++)
                {
                    for (int y = 0; y < 10; y++)
                    {
                        if (Globals.ship2[x][y] > 0 && Globals.chosen2[x][y] == false) Globals.win1 = false;
                    }
                }
                if (Globals.win1 == true)
                {
                    label4.Visible = false;
                    pictureBox1.Visible = true;
                    MessageBox.Show("Player 1 has won the game!");

                    Form1_Load(sender, e);
                }
                //string h = Globals.hit1.ToString();
                //MessageBox.Show(h);
            }
            if (Globals.hit1 == false && Globals.robot == true && Globals.win1 == false)//robot part
                {
                    label1.BackColor = System.Drawing.Color.Red;
                    label2.BackColor = System.Drawing.Color.Transparent;
                    robotchooseloop(sender, e);
                }
            

            else if (Globals.hit1 == false && Globals.robot == false)//friend vs friend part
            {
                for (int z = 0; z < 10; z++) for (int t = 0; t < 10; t++) if (Globals.chosen2[z][t] == false) Globals.visible2[z][t].Visible = true;
                for (int z = 0; z < 10; z++) for (int t = 0; t < 10; t++) Globals.visible1[z][t].Visible = false;

                label1.BackColor = System.Drawing.Color.Red;
                label2.BackColor = System.Drawing.Color.Transparent;
            }
        }

        void robotchooseloop(object senderZ, EventArgs Z) //main AI function
        {
            int empty = 0;
            int size = 9;
            int ibreak = 0;
            Random rand = new Random();
            Globals.player1 = true;


            //Globals.player1'S TURN
            //INSIDE Globals.player1'S TURN'S CODE - SET Globals.player1 = TRUE
            //ONLY RUN IF TRUE


            if (Globals.decide == 0) //first guess is random spot
            {
                do
                {
                    Globals.row2 = rand.Next(0, 10);
                    Globals.column2 = rand.Next(0, 10);
                } while (Globals.row2 % 2 != Globals.column2 % 2);
                //SEND COORDINATES(row2, column2)
                xiu(senderZ, Z, Globals.row2, Globals.column2);
                if (Globals.win2 == true) return;
                if (Globals.ship1[Globals.row2][Globals.column2] != 0) //hit a ship
                {
                    Globals.shipnum = Globals.ship1[Globals.row2][Globals.column2];
                    Globals.chosen1[Globals.row2][Globals.column2] = true;
                    Hit(senderZ, Z);
                    if (Globals.win2 == true) return;
                }
                else
                {
                    Globals.chosen1[Globals.row2][Globals.column2] = true;
                    Globals.decide = 1;
                }
            }

            else if (Globals.decide == 1) //finding empty spot
            {
                for (int a = size; a > 0; a--) //size of empty space
                {
                    if (a == 1) //if only 1x1 squares left
                    {
                        do
                        {
                            Globals.row2 = rand.Next(0, 10);
                            Globals.column2 = rand.Next(0, 10);
                        } while ((Globals.row2 % 2 != Globals.column2 % 2) || (Globals.chosen1[Globals.row2][Globals.column2] == true));
                        //SEND COORDINATES
                        xiu(senderZ, Z, Globals.row2, Globals.column2);
                        if (Globals.win2 == true) return;
                        if (Globals.ship1[Globals.row2][Globals.column2] != 0) //hit a ship
                        {
                            Globals.shipnum = Globals.ship1[Globals.row2][Globals.column2];
                            Globals.chosen1[Globals.row2][Globals.column2] = true;
                            Hit(senderZ, Z);
                            if (Globals.win2 == true) return;
                        }
                        else
                        {
                            Globals.chosen1[Globals.row2][Globals.column2] = true;
                        }
                    }
                    else
                    {
                        for (int b = 0; b <= (10 - a); b++) //work through rows
                        {
                            for (int c = 0; c <= (10 - a); c++) //work through columns
                            {
                                for (int d = 0; d < a; d++) //row coordinates
                                {
                                    for (int e = 0; e < a; e++) //column coordinates
                                    {
                                        if (Globals.chosen1[d + b][e + c] == true)
                                        {
                                            empty++;
                                        }
                                    }
                                }
                                if (empty == 0)
                                {
                                    //guesses only on diagonal spots since smallest ship size is 2
                                    if ((b % 2 == c % 2) && (a % 2 != 0)) //b and c are both even/odd and size of empty space is odd
                                    {
                                        Globals.row2 = (a / 2) + b;
                                        Globals.column2 = (a / 2) + c;
                                    }
                                    else if ((b % 2 != c % 2) && (a % 2 != 0)) //b and c are even and odd and size of empty space is odd
                                    {
                                        Globals.row2 = (a / 2) + b + 1;
                                        Globals.column2 = (a / 2) + c;
                                    }
                                    else if ((b % 2 == c % 2) && (a % 2 == 0)) //b and c are both even/odd and size of empty space is even
                                    {
                                        Globals.row2 = (a / 2) + b;
                                        Globals.column2 = (a / 2) + c;
                                    }
                                    else //b and c are even and odd and size of empty space is even
                                    {
                                        Globals.row2 = (a / 2) + b;
                                        Globals.column2 = (a / 2) + c - 1;
                                    }
                                    size = a;
                                    //SEND COORDINATES
                                    xiu(senderZ, Z, Globals.row2, Globals.column2);
                                    if (Globals.win2 == true) return;
                                    if (Globals.ship1[Globals.row2][Globals.column2] != 0) //hit a ship
                                    {
                                        Globals.shipnum = Globals.ship1[Globals.row2][Globals.column2];
                                        Globals.chosen1[Globals.row2][Globals.column2] = true;
                                        Hit(senderZ, Z);
                                        if (Globals.win2 == true) return;
                                    }
                                    else
                                    {
                                        Globals.chosen1[Globals.row2][Globals.column2] = true;
                                    }
                                    ibreak = 1;
                                    break;
                                }
                                empty = 0;
                            }
                            if (ibreak == 1)
                            {
                                break;
                            }
                        }
                    }
                    if (ibreak == 1)
                    {
                        break;
                    }
                }
                ibreak = 0;
            }
            else if (Globals.decide == 2) //if hit a ship but not sunk yet
            {
                Hit(senderZ, Z);
                if (Globals.win2 == true) return;
            }

            if (Globals.player1 == false) xiu2(senderZ, Z, 0, 0);
        }

        void Hit(object senderB, EventArgs B) //AI function, if hits a ship
        {
            int gbreak = 0;
            int direction = 0; //always up first, 0 = up, 1 = right, 2 = down, 3 = left
            int shipsunk = 0;
            int space = 0;
            
            //bool Globals.player1 = true;//true = run the players code, false = don't run the players code
            //MessageBox.Show("enters hit");
            //Console.WriteLine("hit function");
            while (true)
            {
                if (direction == 0) //guess up
                {
                    while (true)
                    {
                        //Console.WriteLine("while loop");
                        if (Globals.row2 - space == 0) //if hit a wall 
                        {
                            //Console.WriteLine("if loop");
                            direction = 1;
                            space = 0;
                            break;
                        }
                        else if (Globals.chosen1[Globals.row2 - space - 1][Globals.column2] == true) //if already guessed
                        {
                            //Console.WriteLine("else if loop");
                            direction = 1;
                            space = 0;
                            break;
                        }
                        else //guess
                        {
                            //Console.WriteLine("else loop");
                            space++;

                            //SEND COORDINATES (row2 - space, column2)


                            xiu(senderB, B, (Globals.row2 - space), Globals.column2);
                            if (Globals.win2 == true) return;

                            if (Globals.ship1[Globals.row2 - space][Globals.column2] == 0) //if guessed and nothing there, end turn
                            {
                                Globals.chosen1[Globals.row2 - space][Globals.column2] = true;
                                direction = 1;
                                Globals.decide = 2;
                                gbreak = 1;
                                space = 0;
                                break;
                            }
                            else //check if ship sunk
                            {
                                Globals.chosen1[Globals.row2 - space][Globals.column2] = true;
                                for (int a = 0; a < 10; a++)
                                {
                                    for (int b = 0; b < 10; b++)
                                    {
                                        if (Globals.ship1[a][b] == Globals.shipnum && Globals.chosen1[a][b] == false)
                                        {
                                            shipsunk++;
                                        }
                                    }
                                }


                                if (shipsunk == 0) //ship sunk
                                {
                                    Globals.player1 = false;
                                    direction = 0;
                                    Globals.decide = 1;
                                    space = 0;

                                    //checks if there have been ships that are hit but not fully sunk
                                    for (int a = 0; a < 10; a++)
                                    {
                                        for (int b = 0; b < 10; b++)
                                        {
                                            if (Globals.ship1[a][b] != 0 && Globals.chosen1[a][b] == true) //ships that have been hit
                                            {
                                                Globals.shipnum = Globals.ship1[a][b];
                                                for (int c = 0; c < 10; c++) //checks if ship is fully sunk
                                                {
                                                    for (int d = 0; d < 10; d++)
                                                    {
                                                        if (Globals.ship1[c][d] == Globals.shipnum && Globals.chosen1[c][d] == false)
                                                        {
                                                            shipsunk++;
                                                        }
                                                    }
                                                }
                                                if (shipsunk != 0) //if not fully sunk, send to decide = 2
                                                {
                                                    Globals.row2 = a;
                                                    Globals.column2 = b;
                                                    shipsunk = 0;
                                                    Globals.decide = 2;
                                                    gbreak = 2;
                                                    break;
                                                }
                                                else Globals.player1 = false;
                                            }
                                        }
                                        if (gbreak == 2)
                                        {
                                            break;
                                        }
                                    }


                                    gbreak = 1;
                                    break;
                                }
                                else //ship not sunk
                                {
                                    shipsunk = 0;
                                }
                            }
                        }
                    }
                    if (gbreak == 1)
                    {
                        break;
                    }
                }
                if (direction == 1) //guess right
                {
                    while (true)
                    {
                        if (Globals.column2 + space == 9) //if hit a wall 
                        {
                            direction = 2;
                            space = 0;
                            break;
                        }
                        else if (Globals.chosen1[Globals.row2][Globals.column2 + space + 1] == true) //if already guessed
                        {
                            direction = 2;
                            space = 0;
                            break;
                        }
                        else //guess
                        {
                            space++;
                            //SEND COORDINATES (row2, column2 + space)

                            xiu(senderB, B, Globals.row2, Globals.column2 + space);
                            if (Globals.win2 == true) return;
                            if (Globals.ship1[Globals.row2][Globals.column2 + space] == 0) //if guessed and nothing there, end turn
                            {
                                Globals.chosen1[Globals.row2][Globals.column2 + space] = true;
                                direction = 2;
                                Globals.decide = 2;
                                gbreak = 1;
                                space = 0;
                                break;
                            }
                            else //check if ship sunk
                            {
                                Globals.chosen1[Globals.row2][Globals.column2 + space] = true;
                                for (int a = 0; a < 10; a++)
                                {
                                    for (int b = 0; b < 10; b++)
                                    {
                                        if (Globals.ship1[a][b] == Globals.shipnum && Globals.chosen1[a][b] == false)
                                        {
                                            shipsunk++;
                                        }
                                    }
                                }
                                if (shipsunk == 0) //ship sunk

                                {
                                    Globals.player1 = false;
                                    direction = 0;
                                    Globals.decide = 1;
                                    space = 0;



                                    //checks if there have been ships that are hit but not fully sunk
                                    for (int a = 0; a < 10; a++)
                                    {
                                        for (int b = 0; b < 10; b++)
                                        {
                                            if (Globals.ship1[a][b] != 0 && Globals.chosen1[a][b] == true) //ships that have been hit
                                            {
                                                Globals.shipnum = Globals.ship1[a][b];
                                                for (int c = 0; c < 10; c++) //checks if ship is fully sunk
                                                {
                                                    for (int d = 0; d < 10; d++)
                                                    {
                                                        if (Globals.ship1[c][d] == Globals.shipnum && Globals.chosen1[c][d] == false)
                                                        {
                                                            shipsunk++;
                                                        }
                                                    }
                                                }
                                                if (shipsunk != 0) //if not fully sunk, send to decide = 2
                                                {
                                                    Globals.row2 = a;
                                                    Globals.column2 = b;
                                                    shipsunk = 0;
                                                    Globals.decide = 2;
                                                    gbreak = 2;
                                                    break;
                                                }
                                            }
                                        }
                                        if (gbreak == 2)
                                        {
                                            break;
                                        }
                                    }


                                    gbreak = 1;
                                    break;
                                }
                                else //ship not sunk
                                {
                                    shipsunk = 0;
                                }
                            }
                        }
                    }
                    if (gbreak == 1)
                    {
                        break;
                    }
                }
                if (direction == 2) //guess down
                {
                    while (true)
                    {
                        if (Globals.row2 + space == 9) //if hit a wall 
                        {
                            direction = 3;
                            space = 0;
                            break;
                        }
                        else if (Globals.chosen1[Globals.row2 + space + 1][Globals.column2] == true) //if already guessed
                        {
                            direction = 3;
                            space = 0;
                            break;
                        }
                        else //guess
                        {
                            space++;
                            //SEND COORDINATES (row2 + space, column2)
                            xiu(senderB, B, Globals.row2 + space, Globals.column2);
                            if (Globals.win2 == true) return;
                            if (Globals.ship1[Globals.row2 + space][Globals.column2] == 0) //if guessed and nothing there, end turn
                            {
                                Globals.chosen1[Globals.row2 + space][Globals.column2] = true;
                                direction = 3;
                                Globals.decide = 2;
                                gbreak = 1;
                                space = 0;
                                break;
                            }
                            else //check if ship sunk
                            {
                                Globals.chosen1[Globals.row2 + space][Globals.column2] = true;
                                for (int a = 0; a < 10; a++)
                                {
                                    for (int b = 0; b < 10; b++)
                                    {
                                        if (Globals.ship1[a][b] == Globals.shipnum && Globals.chosen1[a][b] == false)
                                        {
                                            shipsunk++;
                                        }
                                    }
                                }
                                if (shipsunk == 0) //ship sunk
                                {
                                    Globals.player1 = false;
                                    direction = 0;
                                    Globals.decide = 1;
                                    space = 0;



                                    //checks if there have been ships that are hit but not fully sunk
                                    for (int a = 0; a < 10; a++)
                                    {
                                        for (int b = 0; b < 10; b++)
                                        {
                                            if (Globals.ship1[a][b] != 0 && Globals.chosen1[a][b] == true) //ships that have been hit
                                            {
                                                Globals.shipnum = Globals.ship1[a][b];
                                                for (int c = 0; c < 10; c++) //checks if ship is fully sunk
                                                {
                                                    for (int d = 0; d < 10; d++)
                                                    {
                                                        if (Globals.ship1[c][d] == Globals.shipnum && Globals.chosen1[c][d] == false)
                                                        {
                                                            shipsunk++;
                                                        }
                                                    }
                                                }
                                                if (shipsunk != 0) //if not fully sunk, send to decide = 2
                                                {
                                                    Globals.row2 = a;
                                                    Globals.column2 = b;
                                                    shipsunk = 0;
                                                    Globals.decide = 2;
                                                    gbreak = 2;
                                                    break;
                                                }
                                            }
                                        }
                                        if (gbreak == 2)
                                        {
                                            break;
                                        }
                                    }


                                    gbreak = 1;
                                    break;
                                }
                                else //ship not sunk
                                {
                                    shipsunk = 0;
                                }
                            }
                        }
                    }
                    if (gbreak == 1)
                    {
                        break;
                    }
                }
                if (direction == 3)//guess left
                {
                    while (true)
                    {
                        if (Globals.column2 - space == 0) //if hit a wall 
                        {
                            //direction = 2;
                            space = 0;
                            break;
                        }
                        else if (Globals.chosen1[Globals.row2][Globals.column2 - space - 1] == true) //if already guessed
                        {
                            //direction = 2;
                            space = 0;
                            break;
                        }
                        else //guess
                        {
                            space++;
                            //SEND COORDINATES (row2, column2 - space)
                            xiu(senderB, B, Globals.row2, Globals.column2 - space);
                            if (Globals.win2 == true) return;
                            if (Globals.ship1[Globals.row2][Globals.column2 - space] == 0) //if guessed and nothing there, end turn
                            {
                                Globals.chosen1[Globals.row2][Globals.column2 - space] = true;
                                //direction = 2;
                                gbreak = 1;
                                space = 0;
                                break;
                            }
                            else //check if ship sunk
                            {
                                Globals.chosen1[Globals.row2][Globals.column2 - space] = true;
                                for (int a = 0; a < 10; a++)
                                {
                                    for (int b = 0; b < 10; b++)
                                    {
                                        if (Globals.ship1[a][b] == Globals.shipnum && Globals.chosen1[a][b] == false)
                                        {
                                            shipsunk++;
                                        }
                                    }
                                }
                                if (shipsunk == 0) //ship sunk
                                {
                                    Globals.player1 = false;
                                    direction = 0;
                                    Globals.decide = 1;
                                    space = 0;



                                    //checks if there have been ships that are hit but not fully sunk
                                    for (int a = 0; a < 10; a++)
                                    {
                                        for (int b = 0; b < 10; b++)
                                        {
                                            if (Globals.ship1[a][b] != 0 && Globals.chosen1[a][b] == true) //ships that have been hit
                                            {
                                                Globals.shipnum = Globals.ship1[a][b];
                                                for (int c = 0; c < 10; c++) //checks if ship is fully sunk
                                                {
                                                    for (int d = 0; d < 10; d++)
                                                    {
                                                        if (Globals.ship1[c][d] == Globals.shipnum && Globals.chosen1[c][d] == false)
                                                        {
                                                            shipsunk++;
                                                        }
                                                    }
                                                }
                                                if (shipsunk != 0) //if not fully sunk, send to decide = 2
                                                {
                                                    Globals.row2 = a;
                                                    Globals.column2 = b;
                                                    shipsunk = 0;
                                                    Globals.decide = 2;
                                                    gbreak = 2;
                                                    break;
                                                }
                                            }
                                        }
                                        if (gbreak == 2)
                                        {
                                            break;
                                        }
                                    }


                                    gbreak = 1;
                                    break;
                                }
                                else //ship not sunk
                                {
                                    shipsunk = 0;
                                }
                            }
                        }
                    }
                    if (gbreak == 1)
                    {
                        break;
                    }
                }
            }
            gbreak = 0;
            
        }
        //have to have these empty loops because otherwise the designer page doesn't work
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void pinkUnselectedSpaceToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)//this is for player 2!!
        {

        }

        private void label3_Click(object sender, EventArgs e)//for player 1
        {

        }

        private void background_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            click.Play();
            pictureBox2.Visible = false;
            title.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            click.Play();
            Globals.robot = true;
            pictureBox2.Visible = false;
            title.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {

        }

        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.URL = @"BGM.wav";
        }
        //static void Main()
        //{
        //    Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault(false);
        //    Application.Run(new Form1());
        //    foreach (int i in Enumerable.Range(0, 10))
        //    {
        //        foreach (int j in Enumerable.Range(0, 10))
        //        {
        //            Button b = new Button();
        //            b.Size = new System.Drawing.Size(20, 20);
        //            b.Location = new Point(i * 20, j * 20);
        //            b.Click += new EventHandler(anyButton_Click); // <-- all wired to the same handler
        //            this.Controls.Add(b);
        //        }
        //    }
        //}
    }
    //global values
    public static class Globals
    {
        public static List<List<int>> ship1 = new List<List<int>>();
        public static List<List<int>> ship2 = new List<List<int>>();
        public static List<List<bool>> chosen1 = new List<List<bool>>();//stores the buttons a person clicks
        public static List<List<bool>> chosen2 = new List<List<bool>>();
        public static int shipchoose = 1;
        public static int chooseturn = 1;
        public static int turn = 1;
        public static bool hit1 = true;
        public static bool hit2 = true;
        public static List<List<Button>> empty1 = new List<List<Button>>();
        public static List<List<Button>> empty2 = new List<List<Button>>();
        public static List<List<Button>> cover1 = new List<List<Button>>();
        public static List<List<Button>> cover2 = new List<List<Button>>();
        public static List<List<Button>> visible1 = new List<List<Button>>();
        public static List<List<Button>> visible2 = new List<List<Button>>();
        public static bool initial = true;
        public static bool robot = false;//checks if player chooses to enter robot mode
        public static int row2 = 0;
        public static int column2 = 0;
        public static int decide = 0;
        public static int shipnum = 0;
        public static bool player1 = true;
        public static bool win1 = true;
        public static bool win2 = true;

    }
}
