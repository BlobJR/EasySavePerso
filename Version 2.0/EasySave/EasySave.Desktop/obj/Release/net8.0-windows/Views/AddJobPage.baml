   M S B A M L   `   `   `  ����   EasySave.Desktop'    !EasySave.Desktop.Views.AddJobPage   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  Ajouter un Job��$	��350��$	��400�� � WindowStartupLocation$ CenterScreen=�.��� $��20q� � RowDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $��* �.+��� $��0��$��Nom du job :��$��5q��� $��1��$��5q����� *	JobName+�� $��2��$��Chemin Source :��$��5q��� $��3��$��5q����� *
SourcePath+�� $��4��$��Chemin Destination :��$��5q��� $��5��$��5q����� *DestinationPath+�� $��6��$?��9Type de sauvegarde (1 = Complète, 2 = Différentielle) :��$��5q��� -   $��7��$��5q����� *	SaveTypes+ �� SelectedItem �� *SelectedSaveType+�� $��8��$��Ajouter��$��10q�$��Center=����� *AddJobCommand+