   M S B A M L   `   `   `  ����   	Boardcast    BoardCast.ToolsWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -     � WindowStartupLocation$  Manual=� � Left$ 0��
 � Top$ 0�� � Title$ toolsWindow�� � WindowState$ 	Maximized=� � WindowStyle$
 None=� � UseLayoutRounding . &   System.Windows.Media.TextOptions   TextFormattingMode$ Display=� � Topmost . 	 � 
ResizeMode$	 NoResize=�
 � AllowsTransparency
 . $
��Auto��$	��450����Z�  � 	Resources    	DarkBrush �� Color  
LightBrush  ColourBrush  LinearGradient_Orange_H �� EndPoint �� 
StartPoint � Color � Offset  Horizontal_Gradient  Vertical_Gradient  MouseOverBrush �� 
TargetType �� Property �� Value  NormalBorderBrush �� 
TargetType �� 
Background �� Bullet ��Name T� X T� Y �� Triggers P� Property P� Value N� EnterActions �� 
TargetName g� Duration  �� TargetProperty! W� From	" W� To# W� EasingFunction4  .System.Windows.Media.Animation.ExponentialEase$   Exponent7  1System.Windows.Media.Animation.EasingFunctionBase%   
EasingMode& N� ExitActions  MyFocusVisual' �� StrokeDashArray( � ActualWidth) � ActualHeight* �� CornerRadius 	 ShadowStyle+ �� Padding, �� 
TargetName 
 highlightedButtonStyle  ScrollViewerStyle�� D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     %'  &	        &	      &	 &     &	 9     &	 �     &	 !    &	 q    '�� �    &	 F    &		 �    '�� �    7  7   7  7   7  7  7   &	
 -
    7  7   7  7   &	     �� $ #050505���� $ #FDFDFD���� $ #1ba1e2���� $	 0,1&�$	 0,0&�.��� $ 	#FFF9FF09��$ 0Q�� $ 	#FFF7DE25��$ 0.959Q�� $ 	#FFFF7000��$ 0.033Q�� $ 	#FFFF7200��$ 1Q��� $	 1,0&�$	 0,0&�.��� $
 #CCC��$	 0.0Q�� $
 #444��$	 1.0Q��� $	 0,1&�$	 0,0&�.��� $
 #CCC��$	 0.0Q�� $
 #444��$	 1.0Q��� $	 0,1&�$	 0,0&�.��� $
 #FFF��$	 0.0Q�� $
 #AAA��$	 1.0Q��� " ��.���  �@J�SnapsToDevicePixels . ��  � ��$ 2q���  � ��# �   ��  � ��# �  ��  � �� �� " ��.R��� 
 ����  � $��26��$��16��.+��� $ Border��#���   #���  $��1,1,1,1q�$	��0,2q��� $ ColourBorder��$��0��#���  #���  $��1,1,1,1q�$
��Left=�$	��0,2q��� $ SwitchBorder��#���  #���   $��1,1,1,1q�$
��Left=�$��10��z�T� $ 0Q�$ 0Q�._��� #��z��#��z��#��z����.  P�  �@d�	IsChecked$ falseY� �� .���� .��W� $ SwitchBorder��$ 0:0:0.2D�$,  &RenderTransform.(TranslateTransform.X)�$! 16X�$" 0X�#   $$ 12Q�$% EaseOut=�W� $ ColourBorder��$ 0:0:0.2D�$  Width�$! 26X�$" 0X�#   $$ 12Q�$% EaseOut=�& �� .���� .��W� $ SwitchBorder��$ 0:0:0.0D�$,  &RenderTransform.(TranslateTransform.X)�W� $ ColourBorder��$ 0:0:0.0D�$  Width�P�  �@d�	IsChecked$
 trueY� �� .���� .��W� $ SwitchBorder��$ 0:0:0.2D�$,  &RenderTransform.(TranslateTransform.X)�$! 0X�$" 16X�#   $$ 12Q�$% EaseOut=�W� $ ColourBorder��$ 0:0:0.2D�$  Width�$! 0X�$" 26X�#   $$ 12Q�$% EaseOut=�& �� .���� .��W� $ SwitchBorder��$ 0:0:0.0D�$,  &RenderTransform.(TranslateTransform.X)�W� $ ColourBorder��$ 0:0:0.0D�$  Width�P�  �@J�IsMouseOver . P�  � �� . P�  � {� .  �� .���  � �� �� " ��.R�� $	��3,2q�.+��� $��r1��$��1��
���   �$	' 2 2R��� $��border��#��z( #��z) $* 2��$��1q��� .���  � ��
 ������� " ��.���  �@J�SnapsToDevicePixels . ��  �@�OverridesDefaultStyle . ��  � ��$ 2q���  � ��$
 11px(���  �@�FocusVisualStyle8   ��  � ��8  ��  � ��8  ��  � �� �� " ��.R��� $��border��$��2q�$	+ 3,1q�8�� $* 0��#��z��._�� .+��� $��content��$��Center=�$��Center=� P�  �@J�IsMouseOver . . ��� $, border��8   � ��P�  � �� . . ��� 8   � ���� 8   � ��P�  �@��IsDefaulted . P�  �@J�	IsFocused . P�  � {� .  �� " ��.���  �@J�SnapsToDevicePixels . ��  �@�OverridesDefaultStyle . ��  � ��$ 2q���  � ��$
 11px(���  �@�FocusVisualStyle8   ��  � �� �� " ��.R��� $��border��$��0 0 0 4q�$+ 0q�8�� $* 0��#��z��._�� .+��� $��content��$��Center=�$��Center=� P�  �@J�IsMouseOver . P�  � �� . . ��� 8   � ���� 8   � ��P�  �@��IsDefaulted . P�  �@J�	IsFocused . P�  � {� .  �� " ��.���  � ��
 �   ���  � ��$ Visible=���  � ��$ Hidden=�.��]� $��-1,0,0,0q�$��Stretch=�$��Bottom=�.+��� -   $	��Top=�$��1q�}�. - J� RenderTransformOrigin$- 0.5,1&�._�� .+�]� $��Stretch=�.+�]� $��Bottom=�
���   �.+�� -   $��Right=�$��22��$��22��
���  ��. � Cursor$
. Hand��.+�(� .���� $	��100��$	��100��.+��� 	/ �� X1$/ 25��	0 �� Y1$0 25��	1 �� X2$1 75��	2 �� Y2$2 75��
�������$��14���� $/ 75��$0 25��$1 25��$2 75��
�������$��14��� .+��� 3 �� Orientation$3 
Horizontal=�.+��� -   $��hideInkCheckBox��4 � ToolTip$4 
(Ctrl + 1)��$
��Left=�$��Center=�$��4,0,0,0q�
�������$
. Hand���� $��Hide ink��
�������$��Center=�$��3,0,0,0q��� $ Ideal=�$��	Boardcast��
�������$��Center=�$��6,-2,6,0q�5 �� TextAlignment$5 Right=�$��Right=�$��16(�$
��Bold"�$��Italic$�]� -   $��toolsDockPanel����. $��Stretch=�$
��Auto��$��	0,-20,0,1q�.+��� -   $��selectedColourBorder��$��Right=�
���   �$��0��$��36��$��Bottom=��� $��Stretch=�$
��Auto��$��0,0,0,0q�.+�� -   $��SubMenusGrid��$	��170��.+��� -   6 ��Name$6 PowerPointPanelRight��$3 
Horizontal=�$��Bottom=�$��Right=�$y�Hidden=�.+��� -	   7 ��Name$7 
backPPTBtn��$��40��$��40��$
. Hand��
������ $��0q�#��[" �.���� $��icons/arrow_back.png���� -
   $7 forwardPPTBtn_Copy��$��40��$��40��$
. Hand��$��0q�#��[" �
������ .���� $��icons/arrow_next.png���� -   $7 exitPPTBtn_Copy1��$��45��$��45��$
. Hand��$��0q�#��[" �
������ .���� $��icons/cancel.png��$��Center=��� -   $6 PowerPointPanelLeft��$3 
Horizontal=�$��Bottom=�$
��Left=�$y�Hidden=�.+��� -   $7 backPPTBtn1��$��40��$��40��$
. Hand��
������ $��0q�#��[" �.���� $��icons/arrow_back.png���� -   $7 forwardPPTBtn_Copy1��$��40��$��40��$
. Hand��$��0q�#��[" �
������ .���� $��icons/arrow_next.png���� -   $7 exitPPTBtn_Copy3��$��45��$��45��$
. Hand��$��0q�#��[" �
������ .���� $��icons/cancel.png���� -   $6 deleteStrokePanel��$3 
Horizontal=�$��Bottom=�$��Center=�$��	0,0,155,0q�$y�Hidden=�.+��� -   $7 exitPPTBtn_Copy2��$��28��$��28��$��0,0,0,0q�$
. Hand��.���� $��icons/closebar.png��$��22��$��17��$��Center=��� -   $6 
colorPanel��$3 
Horizontal=�$��Center=�$��20��$	��284��$��Bottom=�$y�Hidden=�.+��� -   $��18��$��18��
���   �$��0,0,0,0q�$
. Hand���� -   $��18��$��18��
���  ��$��1,0,0,0q�$
. Hand���� -   $��18��$��18��
����  �$��1,0,0,0q�$
. Hand���� -   $��18��$��18��
��� � �$��1,0,0,0q�$
. Hand���� -   $��18��$��18��
��� ���$��1,0,0,0q�$
. Hand���� -   $��18��$��18��
���**��$��1,0,0,0q�$
. Hand���� -   $��18��$��18��
���� ��$��1,0,0,0q�$
. Hand���� -   $��18��$��18��
��� ���$��1,0,0,0q�$
. Hand���� -   $��18��$��18��
���""��$��1,0,0,0q�$
. Hand���� -   $��18��$��18��
�������$��1,0,0,0q�$
. Hand���� -   $��18��$��18��
�������$��1,0,0,0q�$
. Hand���� -   $��18��$��18��
�����p�$��1,0,0,0q�$
. Hand���� -   $��18��$��18��
������$��1,0,0,0q�$
. Hand���� -    $��18��$��18��
���� �$��1,0,0,0q�$
. Hand���� -!   $��18��$��18��
���2͚�$��1,0,0,0q�$
. Hand���� -"   $6 brushSizeStackPanel��$3 
Horizontal=�$	��120��$��Center=�$��Bottom=�$��39��$��324,0,323,0q�$y�Hidden=�.+��� -#   $7 smallestSize��#��[
 $��28��$��28��$��0,0,0,0q�$
. Hand��
�������.��A� $��2��$��2������ *
Foreground+8 �� RelativeSource8 �� *FindAncestor+9 �� AncestorLevel$9 1��: �� AncestorType": ���� -$   $7 	smallSize��$��28��$��28��$��1,0,0,0q�$
. Hand��
�������$��0q�#��[" �.��A� $��5��$��5������ *
Foreground+8 �� *FindAncestor+$9 1��": ���� -%   $7 
mediumSize��$��28��$��28��$��1,0,0,0q�$
. Hand��
�������$��0q�#��[" �.��A� $��10��$��10������ *
Foreground+8 �� *FindAncestor+$9 1��": ���� -&   $7 	largeSize��$��28��$��28��$��1,0,0,0q�$
. Hand��
�������$��0q�#��[" �.��A� $��16��$��16������ *
Foreground+8 �� *FindAncestor+$9 1��": ���� -'   ; ��Name$; LastShotsScrollViewer��$��Hidden=�$��Visible=�$
��Auto��$��Bottom=�$y�Hidden=�.���� -(   < ��Name$< itemsControl��
���   ����� .R��� $3 
Horizontal=�
���   ��� -)   = ��Name$= WinApplicationsLeft��
���   �$��Bottom=�$
��Left=�> J� Opacity$> 0Q�$	��170��$��0,-3,0,0q�$y�Hidden=�.@��� -*   ? ��Name$? BrowserOpen��$��Verdana)�$��12(�$
��Bold"�$��Bottom=�$��Stretch=�$��0,20,0,0q�.���� $3 
Horizontal=�.+��� $��icons/chrome.png��$��30���� $��Browser��
�������$��Center=�$��Right=�$��5,0,0,0q��� -+   $? FileOpen��$��Verdana)�$��12(�$
��Bold"�$��Bottom=�$��Stretch=�.���� $3 
Horizontal=�.+��� $��icons/file.png��$��30��$��27���� $��	Open file��
�������$��Center=�$��5,0,0,0q��� -,   $? OpenImge��$��Verdana)�$��12(�$
��Bold"�$��Bottom=�$��Stretch=�.���� $3 
Horizontal=�.+��� $��icons/gallery-icon.png��$��30���� $��
Open Image��
�������$��Center=�$��5,0,0,0q��� --   $? PowerPointOpen��$��Verdana)�$��12(�$
��Bold"�$��Bottom=�$��Stretch=�.���� $3 
Horizontal=�.+��� $��icons/powerpoint.png��$��30���� $��Power Point��
�������$��Center=�$��5,0,0,0q��� -.   $= WinApplicationsRight��
���   �$��Bottom=�$��right=�$> 0Q�$	��170��.@��� -/   $? BrowserOpen1��$��Verdana)�$��12(�$
��Bold"�$��Bottom=�$��Stretch=�$��0,20,0,0q�.���� $3 
Horizontal=�.+��� $��icons/chrome.png��$��30���� $��Browser��
�������$��Center=�$��Right=�$��5,0,0,0q��� -0   $? 	FileOpen1��$��Verdana)�$��12(�$
��Bold"�$��Bottom=�$��Stretch=�.���� $3 
Horizontal=�.+��� $��icons/file.png��$��30��$��27���� $��	Open file��
�������$��Center=�$��5,0,0,0q��� -1   $? 	OpenImge1��$��Verdana)�$��12(�$
��Bold"�$��Bottom=�$��Stretch=�.���� $3 
Horizontal=�.+��� $��icons/gallery-icon.png��$��30���� $��
Open Image��
�������$��Center=�$��5,0,0,0q��� -2   $? PowerPointOpen1��$��Verdana)�$��12(�$
��Bold"�$��Bottom=�$��Stretch=�.���� $3 
Horizontal=�.+��� $��icons/powerpoint.png��$��30���� $��Power Point��
�������$��Center=�$��5,0,0,0q��� -3   $6 shapesStackPanel��$3 
Horizontal=�$	��155��$��Center=�$��Bottom=�$��35��$��434,0,178,0q�
������ $y�Hidden=�.+��� -4   $7 CircleShape��
�������$��28��$��28��$��0,0,0,0q�$
. Hand��.���� $��icons/oval.png��$��28���� -5   $7 RectangleShape��
�������$��28��$��28��$��0,0,0,0q�$
. Hand��.���� $��icons/rectangle.png��$��28���� -6   $7 TriangleShape��
�������$��28��$��28��$��0,0,0,0q�$
. Hand��.���� $��icons/triangle.png��$��28���� -7   $7 	LineShape��
�������$��28��$��28��$��0,0,0,0q�$
. Hand��.���� $��icons/minus.png��$��28���� -8   $7 CoordinateShape��
�������$��28��$��28��$��0,3.5q�$
. Hand��.���� $��icons/signs.png��$	��30 ���� -9   $; LastStrokesScrollViewer��$��Hidden=�$��Visible=�$
��Auto��$��Bottom=�$y�Hidden=�.���� -:   $< strokesItemsControl1��
���   ����� .R��� $3 
Horizontal=�
���   �� ���#B5B5B5.+��� -;   $6 openWinAppsPannel��$3 
Horizontal=�$��Bottom=�$
��Auto��$
��Left=�$��36��.+��� -<   $��	openFiles��$
. Hand��$��31��$
��Left=�$��Center=�$��35��$- 0.5,0.5&�$��2,0,0,0q�
������ $��0q�#��[" �.���� $��icons/apps.png��$��25��$��25���� -=   $6 toolStackPanel��$3 
Horizontal=�$��Bottom=�$
��Auto��$��Center=�$��36��.+��� ->   $7 cursorButton��$
. Hand��#��[
 $��Center=�$��Center=�$4 Cursor��$��0,2,2,2q���.  $��32��$��35��
������ .���� $��icons/arrow.png��$��17��$��17���� -?   $7 	penButton��$
. Hand��$��Center=�$��Center=�$	4 Pen��
������ $��0q�#��[" �.���� $��icons/pen.png��$��22��$��26���� -@   $7 highlighterButton��$
. Hand��$��Center=�$��Center=�$4 Highlighter��
������ $��0q�#��[" �.���� $��icons/marker.png��$��22��$��26���� -A   $7 eraserButton��$
. Hand��$��Center=�$��Center=�$4 Eraser��
������ $��0q�#��[" �.���� $��icons/erease.png��$��25��$��25���� -B   $7 
editbutton��$
. Hand��$��Center=�$��Center=�$4 Eraser��
������ $��0q�#��[" �.���� $��icons/edit.png��$��25��$��25���� -C   $7 	brushSize��$��35��$��31��$��2q�$
. Hand��$4 
Brush size��
������ $��0q�#��[" �.��A� $��15��$��15������ *
Foreground+8 �� *FindAncestor+$9 1��": ���� -D   $7 penColor��$��35��$��31��$
. Hand��$��0q�#��[" �$4 	Pen color��
������ .���� -E   $��SelectedColorRect��
���   �
���   �$	��100��$	��100���� $��1��$��30��
�������$��8,0,7,0q��� -F   $7 createBlankBackground��$
. Hand��$
��Left=�$��Center=�$��31��$��35��$4 open new blank background��$��2,2.5q�
������ $��0q�#��[" �.���� $��icons/board.png��$��25��$��25��$- -5.404,0.477&�$
��Fill=��� -G   $7 	drawShape��$
. Hand��$
��Left=�$��Center=�$��31��$��35��$4 
Draw shape��
������ $��0q�#��[" �.���� $��icons/shape.png��$��25��$��25��$- -5.404,0.477&�$
��Fill=��� -H   $7 CaptureButton��$
. Hand��$��Center=�$��Center=�$4 Screen Capture��$��31��$��35��
������ $��0q�#��[" �.���� $��icons/camera.png��$��22��$��26���� $��1��$��30��
�������$��8,3,7,3q��� -I   $7 LoadLastShotsButton��$
. Hand��$
��Left=�$��Center=�$��31��$��35��$4 Load last screenshots��$��2,2.5q�
������ $��0q�#��[" �.���� $��icons/gallery.png��$��25��$��25��$- -5.404,0.477&�$
��Fill=��� -J   $7 LoadLastStrokesButton��$
. Hand��$
��Left=�$��Center=�$��31��$��35��$4 Load last notes��$��2,2.5q�
������ $��0q�#��[" �.���� $��icons/gallery.png��$��25��$��25��$- -5.404,0.477&�$
��Fill=��� -K   $7 eraseAllButton��$
. Hand��$��Center=�$��Center=�$4 	Erase all��
������ $��0q�#��[" �.���� $��icons/clean.png��$��25��$��25���� -L   $7 KeyboardButton��$
. Hand��$��Center=�$��Center=�$4 Keyboard��
������ $��0q�#��[" �$��0,3.5q�.���� $��icons/keyboard.png��$��27��$��25���� -M   $6 openWinAppsPannel_Copy��$3 
Horizontal=�$��Bottom=�$
��Auto��$��Right=�$��36��.+��� -N   $7 
openFiles1��$
. Hand��$��31��$
��Left=�$��Center=�$��35��$- 0.5,0.5&�$��2,0,0,0q�
������ $��0q�#��[" �.���� $��icons/apps.png��$��25��$��25��� 