   M S B A M L   `   `   `  ���� K  GEasySave.Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:EasySave.Desktop.PropertiesEasySave.Desktop.Properties  %    EasySave.Desktop.Views.HomePage   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     :resx)clr-namespace:EasySave.Desktop.Properties       � Title$
  Home��$	��600��$	��800�� � WindowStartupLocation$ CenterScreen=�.��� $��20q� � RowDefinitions �� $
��Auto ��� $��2* ��� $��1* �.+���  �� Orientation$ 
Horizontal=�$��0��$��Center=�.+��� *   $EasySave.Desktop.Properties.Resource   AddJob#��Z $��5q����� *AddJobCommand+��    EditJob#��Z $��5q����� *EditJobCommand+��    	DeleteJob#��Z $��5q����� *DeleteJobCommand+��    
ExecuteJob#��Z $��5q����� *ExecuteJobCommand+��    JobsListTitle#��Z $��1��$��5q�.���� -   	 ��Name$	 JobListView��$	��200������ *Jobs+
 �� SelectedItem
 �� *SelectedJob+ �� View �� .P��� $	��Nom�� �� Width$	 200�� �� DisplayMemberBinding �� *Name+�� $��Source��$	 250�� �� *
SourcePath+�� $��Destination��$	 250�� �� *DestinationPath+�� $
��Type��$	 100�� �� *
SaveType+��    ConsoleLogTitle#��Z $��2��$��5q�.���� -    ��Name$ ConsoleLogBox��$	��100�� �� 
IsReadOnly$
 True�� �� TextWrapping$
 Wrap=�" �� VerticalScrollBarVisibility$
 Auto=����� *
ConsoleLog+ �� Mode$ OneWay=��� $ 
Horizontal=�$��Right=�$��10q�.+��� $��FR��$��fr��$��5q����� *ChangeLanguageCommand+�� $��EN��$��en��$��5q����� *ChangeLanguageCommand+ � -     �Name$ 	MainFrame��$��1��  � NavigationUIVisibility$ Hidden=�