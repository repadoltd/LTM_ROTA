namespace REP_GET_SMS_PROG
{
    public partial class Form1 : Form
    {
        private TextBox text1 = new();
        private TextBox text2 = new();
        public Form1(string parameter1, string parameter2)
        {
            
            InitializeComponent();
            textBox1.Text = parameter1;
            textBox2.Text = parameter2;



        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}