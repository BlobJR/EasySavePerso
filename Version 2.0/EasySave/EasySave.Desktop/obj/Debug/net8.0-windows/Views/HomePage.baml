   M S B A M L   `   `   `  ����K  GEasySave.Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:EasySave.Desktop.PropertiesEasySave.Desktop.Properties  5   	   ?$clr-namespace:EasySave.Desktop.UtilsEasySave.Desktop.Utils  5   	   %    EasySave.Desktop.Views.HomePage   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   :resx)clr-namespace:EasySave.Desktop.Properties     5   	   6local$clr-namespace:EasySave.Desktop.Utils     5   	     � Title$
  Home��5   	   $	��800��6   $
��1000��6#    � WindowStartupLocation$ CenterScreen=�5   	   $	��600��6.   $	��400��6=    � 	Resources 5	      .   (EasySave.Desktop.Utils.UILanguageManager   LangManager��%   &	          5
   
   6"   5      .��5      � $��20q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $��5 �6   �� 5      $��* �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $
��Left=�6.   $��5q�6I   .+�5      �� $
��JSON��6   $
��JSON��6L   $��60��6d   $��30��6o   $��5q�6{   ��6$   �� *SetLogFormatCommand+6{   �� 5      $	��XML��6   $	��XML��6K   $��60��6b   $��30��6m   $��5q�6y   ��6#   �� *SetLogFormatCommand+6y   5      �� 5    
   $ 
Horizontal=�6   $��Right=�6.   $��5q�6J   .+�5!      �� -   5        $��FR��5!      $��50��6;   $��30��6F   $��5q�6R   �� 5"      -   5        $��EN��5"      $��50��6;   $��30��6F   $��5q�6R   5#      �� 5&   
   $ 
Horizontal=�6   $��0��6.   $��Center=�6;   .+�5'      �� $��5q�6�   ��6   ��  �� Source# [   �� Path$ AddJobLabel���6_   �� *AddJobCommand+6�   �� 5(      $��5q�6�   ��6   �� # [  $ EditJobLabel���6`   �� *EditJobCommand+6�   �� 5)      $��5q�6�   ��6   �� # [  $ DeleteJobLabel���6b   �� *DeleteJobCommand+6�   �� 5*      $��5q�6�   ��6   �� # [  $ ExecuteJobLabel���6c   �� *ExecuteJobCommand+6�   �� 5+      $��5q�6�   ��6   �� # [  $ ExecuteSelectedJobLabel���6k   �� *ExecuteMultipleJobsCommand+6�   �� 5,      $��5q�6�   ��6   �� # [  $" EncryptionConfigurationLabel���6p   �� *ConfigureEncryptionCommand+6�   �� 5-      $��5q�6�   ��6   �� # [  $ DecryptLabel���6`   �� *DecryptFilesCommand+6�   5.      �� 51   
   $��1��6^   $��5q�6k   ��6   �� # [  $ JobsListTitle�.��52      � .+�53      �� -   5         ��Name$ JobListView��53       �� SelectionMode$ Extended=�56      $��Stretch=�58      $��Stretch=�59      ��54      �� *Jobs+	 �� SelectedItem	 55      �� *SelectedJob+
 �� View
 5:      �� 5;      .P�5<      ��  �� Width$	 200��6�   ��6-   �� # [  $ JobName� �� DisplayMemberBinding 6r   �� *Name+6�   �� 5=      $	 250��6�   ��6-   �� # [  $ 	JobSource� 6t   �� *
SourcePath+6�   �� 5>      $	 250��6�   ��6-   �� # [  $ JobDestination� 6y   �� *DestinationPath+6�   �� 5?      $	 100��6�   ��6-   �� # [  $ JobType� 6r   �� *
SaveType+6�   5@      5B      5C      5D      �� 5G   
   $��2��6   $��5��6$   
�������$��Stretch=�6A   $��Center=�6_   �� 5J   
   $��3��6`   $��5q�6m   ��6   �� # [  $ ConsoleLogTitle�.��5K      � .+�5L      �� -   5         ��Name$ ConsoleLogBox��5L      $��Stretch=�5M      $��Stretch=�66   " �� VerticalScrollBarVisibility$
 Auto=�5N       �� TextWrapping$
 Wrap=�6=    �� 
IsReadOnly$
 True��5O      ��5L   1   �� *
ConsoleLog+ �� Mode$ OneWay=�5O      5P      5Q      5R      5S      