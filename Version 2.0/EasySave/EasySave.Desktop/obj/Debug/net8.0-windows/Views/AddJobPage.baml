   M S B A M L   `   `   `  ����K  GEasySave.Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:EasySave.Desktop.UtilsEasySave.Desktop.Utils  5   	   '    !EasySave.Desktop.Views.AddJobPage   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   6local$clr-namespace:EasySave.Desktop.Utils     5   	     � Title$	  Add��5   	   $	��350��6   $	��400��6"    � WindowStartupLocation$ CenterScreen=�5   	    � 	Resources 5      .   (EasySave.Desktop.Utils.UILanguageManager   LangManager��%   &	          5	   
   6"   5
      .��5      � $��20q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��5q�6b   ��6!   �� *
AddJobName+ �� Source# [  6b   �� 5   
   $��1��6   $��5q�68   ��6   �� *	JobName+68   �� 5   
   $��2��6   $��5q�6d   ��6!   �� *AddJobSource+# [  6d   �� 5   
   $��3��6   $��5q�6;   ��6   �� *
SourcePath+6;   �� 5    
   $��4��6   $��5q�6i   ��6!   �� *AddJobDestination+# [  6i   �� 5!   
   $��5��6   $��5q�6@   ��6   �� *DestinationPath+6@   �� 5#   
   $��6��6   $��5q�6b   ��6!   �� *
AddJobType+# [  6b   �� 5$   
   $��7��6   $��5q�6l   ��6    �� *	SaveTypes+ �� SelectedItem 6B   �� *SelectedSaveType+6l   �� 5&   
   -   5        $��8��5&      $��10q�5'   3   $��Center=�6?   ��5&      �� *AddJobButton+# [  ��5'      �� *AddJobCommand+6\   5(      5)      