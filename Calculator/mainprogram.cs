namespace Calculator
{
    public partial class Mainprogram : Form
    {
        public Mainprogram()
        {
            InitializeComponent();
        }
        public float x = 0;
        public string operand = string.Empty;

        private void number_selected(int i) // number button selection adding to label
        {
            if (lblResult.Text == "0") { lblResult.Text = i.ToString(); }
            else { lblResult.Text = lblResult.Text + i.ToString(); }
        }

        private void lbl_0() { lblResult.Text = "0"; } // resetting lbl to 0

        private void calculate(string op) // calculate based off the operand
        {
            try
            {
                float y = float.Parse(lblResult.Text);

                switch (operand)
                {
                    case "+":
                        x = x + y;
                        break;

                    case "-":
                        x = x - y;
                        break;

                    case "*":
                        x = x * y;
                        break;

                    case "/":
                        x = x / y;
                        break;
                }
            }
            catch { } // do nothing, auto reset

            operand = string.Empty;
            lblResult.Text = x.ToString();
        }

        private void Operator_Clicked(string Op) // deals with the operands clicked
        {
            try
            {
                if (operand == string.Empty) // stores new information to deal with
                {
                    operand = Op;
                    x = float.Parse(lblResult.Text);
                }
                else
                {
                    calculate(operand);
                    operand = "+";
                }
                lbl_0();
            }
            catch  // do nothing, auto reset
            {
                
            }
        }

        #region Number_Buttons
        private void btn1_Click(object sender, EventArgs e) { number_selected(1); }

        private void btn2_Click(object sender, EventArgs e) { number_selected(2); }

        private void btn3_Click(object sender, EventArgs e) { number_selected(3); }

        private void btn4_Click(object sender, EventArgs e) { number_selected(4); }

        private void btn5_Click(object sender, EventArgs e) { number_selected(5); }

        private void btn6_Click(object sender, EventArgs e) { number_selected(6); }

        private void btn7_Click(object sender, EventArgs e) { number_selected(7); }

        private void btn8_Click(object sender, EventArgs e) { number_selected(8); }

        private void btn9_Click(object sender, EventArgs e) { number_selected(9); }

        private void btnZero_Click(object sender, EventArgs e) { number_selected(0); }
        #endregion


        #region Operands_Buttons
        private void btnAdd_Click(object sender, EventArgs e) { Operator_Clicked("+"); }

        private void btnEquals_Click(object sender, EventArgs e) { calculate(operand); }

        private void btnMinus_Click(object sender, EventArgs e) { Operator_Clicked("-"); }

        private void btnDivide_Click(object sender, EventArgs e) { Operator_Clicked("/"); }

        private void btnMultiply_Click(object sender, EventArgs e) { Operator_Clicked("*"); }
        #endregion

        #region Other_Buttons
        private void btndot_Click(object sender, EventArgs e)
        {
            lblResult.Text = lblResult.Text + ".";
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            operand = string.Empty;
            lbl_0();
            x = 0;
        }

        private void btnBackspace_Click(object sender, EventArgs e) { lblResult.Text = lblResult.Text.Substring(0, lblResult.Text.Length - 1); }
        #endregion
    }
}
