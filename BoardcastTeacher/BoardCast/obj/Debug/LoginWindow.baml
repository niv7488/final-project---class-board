   M S B A M L   `   `   `  ����  	Boardcast    BoardCast.LoginWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   (   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -   5          � Title$  Login��5   	    � SizeToContent$ WidthAndHeight=�5   	    � WindowStyle$
 None=�5   	    � 
ResizeMode$ NoResize=�5	   	    � WindowStartupLocation$ CenterScreen=�5   	   $	��350��6.   $��450.625��6:   .��5      � $	��400��6   $	��300��6    � RowDefinitions 5   
   �� 5      $	��37* �6   �� 5      $	��20* �6   .+�5   
   �� $��Images/LoginBG.jpg��6   $��UniformToFill=�6,   $��
0,-28,0,-2q�6D   $��2��6X   �� 5   
   $��Images/BCLogo.jpg��6   $��60,19,63,118q�6+   �� 5   
   -   5         ��Name$ wrongeDetails��5   <   $��wronge details��6   $��	0,100,0,0q�6)   
���   �$y�Hidden=�6f   $��Normal&�6z   $��FontAwesome)�6�   $��Normal$�6�   $��Normal"�6�   $��28��6�   $��Center=�6�   $��Center=�6�   $	��126��6  �� 5   
   -   5        $��label2��5   8   $��	User Name��6   $��
14,220,0,0q�6$   $��30��6F   $	��Top=�6R   $
��Left=�6j   $��78��6�   
���   �6�   �� 5   
   -   5        $��txtBxuserName��5   %   $��	85,0,0,12q�6   $��30��6:   $��Center=�6F   $��Bottom=�6h   $
��Left=�6�   $	��193��6�   $��FontAwesome)�6�   
������� J� OpacityMask
 �����
��� ���
���   �6?  �� 5   
   -   5        $��label3��5   #   $��Password��6   $��14,9,0,0q�61   $��30��6C   $	��Top=�6O   $
��Left=�6g   $��80��6�   
���   �$��1��6�   G� 5   
   -   5        $��passBxPassword��5   *   $��
85,0,0,101q�6   $��Center=�6@   $
��Left=�6b   $	��193��6}   $��FontAwesome)�6�   
��� ���
������� G� PasswordChar$ *��6�   	 � OverridesDefaultStyle	 .  
 G� SelectionBrush

 ��  �$��30��6  $��Bottom=�6#  
���   �$��1��6�  6�  �� 5   
   -   5        $��lblfrgtPass��5   @   $��Forgot Password ?��6   $��
100,40,0,0q�6,   
���   �$y�Hidden=�6f   $��Normal&�6z   $��FontAwesome)�6�   $��Normal$�6�   $��Normal"�6�   $��28��6�   $	��Top=�6�   $
��Left=�6�   $	��126��6  $��1��6  �� 5   
   -   5        $��btnStart��5   5   $��Start��6   $��
196,69,0,0q�6!   $y�Hidden=�6\   $��30��6p   $	��Top=�6|   $
��Left=�6�   $��74��6�    � Cursor$
 Hand��6�   $��FontAwesome)�6�   
�������
���   �
���   �$��1��6  �� 5   
   -	   5        $��
lblLoading��5   &   $��Loading ...��6   $y�Hidden=�68   $
��Left=�6L   $��77��6g   $��30��6r   $	��Top=�6~   $��FontAwesome)�6�   $��
116,42,0,0q�6�   $��1��6�   �� 5   
   -
   5        $��btnClose��5   )   $��X��6   $��22��6   $	��Top=�69   
���  �����	#00000000$��0q�6�   $ Arrow��6�    � ForceCursor .   � ToolTip$ Close��6�   $��	276,0,2,0q�6�   
�������6�   �� 5   
   -   5        $��CoursesList��5   (   $��14,176,14,82q�6   $y�Hidden=�6;   $��2��6O   6`   �� 5   
   -   5         ��Name$ 	btnLogout��5   5   $��Logout��6   $��	30,69,0,0q�6"   $y�Hidden=�6H   $��30��6t   $	��Top=�6�   $
��Left=�6�   $��74��6�   $
 Hand��6�   $��FontAwesome)�6�   
�������
���   �
���   �$��1��6   �� 5   
   -   5        $ btnLogin��5   5   $��Login��6   $��
115,69,0,0q�6!   $��30��6^   $	��Top=�6j   $
��Left=�6�   $��74��6�   $
 Hand��6�   $��FontAwesome)�6�   
�������
���   �
���   �$��1��6	  5      5       