using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using VintageGarmentDescriber;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;

namespace VintageGarmentDescriber
{
    #region ImageManupulation

    public class GenericCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        protected MainWindow wnd;

        virtual public bool CanExecute(object parameter)
        {
            wnd = ((MainWindow)parameter);
            return wnd != null;
        }

        virtual public void Execute(object parameter)
        {
            wnd = ((MainWindow)parameter);
        }
    }

    public class LeftCommand : GenericCommand
    {
        #region ICommand Members

        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter); // && wnd.ImageIndex > 0;
        }

        public event EventHandler CanExecuteChanged;

        public override void Execute(object parameter)
        {
            base.Execute(parameter);

            --wnd.ImageIndex;

            wnd.AddedProp.Text = "";

            LoadImgCommand cmd = new LoadImgCommand();
            cmd.Execute(parameter);
        }

        #endregion
    }

    public class RightCommand : GenericCommand
    {
        #region ICommand Members

        public override bool CanExecute(object parameter)
        {
            bool baseRes = base.CanExecute(parameter);
            return baseRes; // &&  filenames != null && filenames.Count <= wnd.ImageIndex + 2;
        }

        public event EventHandler CanExecuteChanged;

        public override void Execute(object parameter)
        {
            base.Execute(parameter);

            //if (filenames != null && filenames.Count < wnd.ImageIndex + 2)
            //    return;

            wnd.AddedProp.Text = "";

            if (wnd.GarmentDescIdx >= wnd.GarmentDescCount-1 && wnd.ImageIndex >= wnd.ImageCount-1)
                return;
            
            wnd.ImageIndex++;
            
            
            LoadImgCommand cmd = new LoadImgCommand();
            cmd.Execute(parameter);

            
        }

        #endregion
    }

    public class OpenFolderCommand : GenericCommand
    {
        #region ICommand Members

        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public override void Execute(object parameter)
        {
            base.Execute(parameter);
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ViewModel.SelectedFolder = "D:\\Photos\\Processed\\SmallPhotos\\FF Online";

            {
                FolderViewModel folder = new FolderViewModel();
                folder.Root = dialog.ViewModel;
                folder.FolderPath = dialog.ViewModel.SelectedFolder;
                folder.FolderName = dialog.ViewModel.SelectedFolder;
                folder.IsSelected = true;
                folder.IsExpanded = true;
                dialog.ViewModel.Folders.Insert(0, folder);
            }


            {
                FolderViewModel folder = new FolderViewModel();
                folder.Root = dialog.ViewModel;
                folder.FolderPath = "D:\\Photos\\Processed\\SmallPhotos";
                folder.FolderName = folder.FolderPath;
                folder.IsExpanded = true;
                dialog.ViewModel.Folders.Insert(1, folder);
            }

            dialog.ViewModel.SelectedFolder = "D:\\Photos\\Processed\\SmallPhotos\\FF Online";

            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                MainWindow wnd = ((MainWindow)parameter);
                wnd.ImageFolder = dialog.SelectedFolder;
            }
        }

        #endregion
    }

    public class LoadImgCommand : GenericCommand
    {
        #region ICommand Members

        static List<String> filenames = new List<string>();

        static public void ClearFileArray()
        {
            filenames.Clear(); 
        }

        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter) && wnd.ImageFolder != String.Empty;
        }

        public event EventHandler CanExecuteChanged;

        public override void Execute(object parameter)
        {
            base.Execute(parameter);
            if (filenames == null || filenames.Count == 0)
            {
                String path = wnd.ImgFolder.Text;
                if (!Directory.Exists(path))
                    return;
                string[] fnArray = Directory.GetFiles(path, "*.jpg");
                for (int i = 0; i < fnArray.Length; ++i)
                {
                    filenames.Add(fnArray[i]);
                }
                filenames.Sort();

                wnd.ImageIndex = 0;
                wnd.ImageCount = filenames.Count;
            }

            if (wnd.ImageIndex + 1 > filenames.Count || wnd.ImageIndex < 0)
                wnd.ImageIndex = 0;

            if (wnd.ImageCount == 0)
                return;

            String filename = filenames[wnd.ImageIndex];
            wnd.FileName.Text = Path.GetFileName(filename);
            
            MainWindow.GarmentImageClass image = new MainWindow.GarmentImageClass(filename);
            wnd.GarmentImage = image;
        }

        #endregion
    }

    #endregion

    #region Garment Properties

    public class GarmentGenericCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        protected List<String> filenames = new List<string>();
        protected Button btn;
        static protected MainWindow wnd;
        
        virtual public bool CanExecute(object parameter)
        {
            btn = ((Button)parameter);
            if (wnd == null)
                wnd = ((MainWindow)Utils.GetTopLevelControl(btn));
            return btn != null;
        }

        virtual public void Execute(object parameter)
        {
            btn = ((Button)parameter);
            if (wnd == null)
                wnd = ((MainWindow)Utils.GetTopLevelControl(btn));

        }

        
    }

    public class GarmentTypeCommand : GarmentGenericCommand
    {
        #region ICommand Members

        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public override void Execute(object parameter)
        {
            base.Execute(parameter);
            
            String type = btn.Content.ToString();
            int lastIdxOfSpace = type.LastIndexOf(' ');
            wnd.AddedProp.Text = btn.Content.ToString().Remove(lastIdxOfSpace, type.Length - lastIdxOfSpace);
            wnd.AddNewDescr();
        }

        #endregion
    }

    #endregion
}
