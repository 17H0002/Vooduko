   M S B A M L   `   `   `  ����F  BProject 102, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:Project_102Project_102  5   	       Project_102.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:Project_102     5   	     � Title$  
MainWindow��5   	   $��345.87��6   $	��525��6,   .��5	      � .+�5
   
   �� $
��Left=�6   $	��236��6-   $��	10,10,0,0q�6:    �� TextWrapping$
 Wrap=�6M   $	��Top=�6a   $	��497��6y   {�.  $����No GUI board at the moment, console should have popped up somewhere. Input the move below and it should reflect on the console.

The "Computer" plays first, generating a random move and position to play. It's move is then run through the different checks for a legal move. If it passes them all, an updated board is printed reflecting the move.

"Player 1" plays second (ironic indeed), inputting a move in the blocks below. Like the computer, if the player's move is legal an updated board reflecting the move is printed. (NOTE: The computer only plays after a move for Player 1 has been played, that's why the console will first appear blank.)

If either player plays an invalid move, the game terminates, declaring the other player the winner. 

Information about the game is printed both to the console, and the block right of the "Play Move" button.

(NOTE: Currently the game doesn't print illegal moves, so it's important to see what the console says. eg: Both player's moves won't print if the Computer's move is illegal, and this will only reflect in the console below.)
��6�   $��10(�6X  �� 5   
   -   5         ��Name$ ColLab��5      $	��Col��6    $
��Left=�6.   $��25��6I   $��
12,246,0,0q�6U   $	��Top=�6i   $��37��6�   �� 5   
   -   5        $ RowLab��5      $	��Row��6    $
��Left=�6.   $��25��6I   $��
54,246,0,0q�6U   $	��Top=�6i   $��37��6�   �� 5   
   -   5         ��Name$ ColText��5      $
��Left=�6#   $��33��6>   $��
12,272,0,0q�6J   $
 Wrap=�6^   $	��Top=�6r   $��37��6�   �� 5   
   -   5        $ RowText��5      $
��Left=�6#   $��33��6>   $��
54,272,0,0q�6J   $
 Wrap=�6^   $	��Top=�6r   $��37��6�   �� 5   
   -   5         ��Name$ PlayMoveBut��5      $��	Play Move��6&   $
��Left=�6:   $��33��6U   $��177,272,0,0q�6a   $	��Top=�6v   $��75��6�   6�   �� 5   
   -   5        $
 Term��5      $
��Left=�6    $��50��6;   $��257,255,0,0q�6G   $
 Wrap=�6\   $	��Top=�6p   $	��250��6�   {�.  6�   �� 5   
   -   5        $ MoveLab��5      $
��Move��6!   $
��Left=�60   $��25��6K   $��100,246,0,0q�6W   $	��Top=�6l   $��46��6�   �� 5   
   -   5        $ MoveText��5      $
��Left=�6$   $��33��6?   $��100,272,0,0q�6K   $
 Wrap=�6`   $	��Top=�6t   $��37��6�   5      5      