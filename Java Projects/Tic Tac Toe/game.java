package tictactoe;

// Imports
import java.awt.GridLayout;

import javax.swing.ImageIcon;
import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JOptionPane;
import javax.swing.JPanel;


public class game extends JPanel{
    // Beginning with X
    char playerchar = 'X';
    int rows = 3;
    int columns = 3;
    int cells_amount = 9;
    char winner = 'N';

    JButton[] jButtons = new JButton[cells_amount];

    public game()
    {
        GridLayout layout = new GridLayout(rows, columns);
        setLayout(layout);

        createButtons();
    }

    public void createButtons()
    {
        for (int i = 0; i < 9; i++)
        {
            jButtons[i] = new JButton();

            jButtons[i].setText(" ");
            // Adding a listener to the new button
            jButtons[i].addActionListener(e -> {
                JButton clickedBtn = (JButton) e.getSource();
                // Setting text to value of the player char
                clickedBtn.setText(String.valueOf(playerchar));

                String img = "";

                if(playerchar == 'X') {
                    playerchar = 'O';
                    img = "cross.png";
                }
                else if(playerchar == 'O'){
                    playerchar = 'X';
                    img = "naught.png";
                }
                // Setting the icons
                ImageIcon ico = new ImageIcon(getClass().getResource(img));
                clickedBtn.setIcon(ico);
                // Removing listener to maintain the bright colour after disabling the button
                clickedBtn.removeActionListener(clickedBtn.getActionListeners()[0]);
                trackPoint(clickedBtn.getText().charAt(0));
            });

            add(jButtons[i]);
        }
    }

    public void trackPoint(char c)
    {
        // check if won
        if(!won(c)) {
            // check if drawn
            if(!checkDraw())
            { return;} // if not won or drawn then back
            else{
                // if drawn
                showText("The game was drawn.", "");
            }
        }
        else // if won
            showText("The winner is ", String.valueOf(winner));

    }

    public void showText(String part1, String part2)
    {
        JOptionPane pane = new JOptionPane();
        int dialog = JOptionPane.showConfirmDialog(pane,
        "Game Over."
        + part1 + part2 + " ",
        "Result", JOptionPane.DEFAULT_OPTION);

        if (dialog == JOptionPane.OK_OPTION)
        System.exit(0);
    }

    public boolean won(char c)
    {
        // if any wins, return win
        boolean didWin = false;

        if(checkRows(c)){return true;}
        if(checkColumns(c)) {return true;}
        if(checkDiagonal(c)) {return true;}

        return didWin;
    }

    public boolean checkDiagonal(char c)
    {
        // diagonal one
        if(jButtons[0].getText().charAt(0) == c && jButtons[4].getText().charAt(0) == c && jButtons[8].getText().charAt(0) == c){
            winner = jButtons[0].getText().charAt(0);
            return true;
        }
        // diagonal two
        else if(jButtons[2].getText().charAt(0) == c && jButtons[4].getText().charAt(0) == c && jButtons[6].getText().charAt(0) == c){
            winner = jButtons[2].getText().charAt(0);
            return true;
        }

        return false;
    }

    public boolean checkRows(char c)
    {
        // index of the start of a next row
        int i = 0;

        for(int row = 0; row < 3; row++)
        {
            // check if within the row all three chars are equal : finding a winner
            if(jButtons[i].getText().charAt(0) == c && jButtons[i+1].getText().charAt(0) == c && jButtons[i+2].getText().charAt(0) == c)
            {
                winner = jButtons[i].getText().charAt(0);
                return true;
            }
            // next row index to look at
            i = i + 3;
        }

        return false;
    }

    public boolean checkColumns(char c)
    {
        // check each column for winner
        for(int col = 0; col < 3; col++)
        {
            if(jButtons[col].getText().charAt(0) == c && jButtons[col + 3].getText().charAt(0) == c && jButtons[col + 6].getText().charAt(0) == c)
            {
                winner = jButtons[col].getText().charAt(0);
                return true;
            }
        }

        return false;
    }

    // Checks if the board has been filled
    public boolean checkDraw()
    {
        for(int i = 0; i < jButtons.length; i++)
        {
            if(jButtons[i].getText() == " "){return false;} // If theres one empty space return
        }
        return true;
    }

    public static void main(String[] args) {
        JFrame jFrame = new JFrame("Tic Tac Toe Game");
    
        jFrame.getContentPane().add(new game());
        jFrame.setBounds(400, 400, 500, 450);
        jFrame.setVisible(true);
        jFrame.setLocationRelativeTo(null);
      }
}
