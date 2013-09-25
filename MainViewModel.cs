using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VintageGarmentDescriber
{
    class MainViewModel
    {
        public MainViewModel()
        {
            LeftCommand = new LeftCommand();
            RightCommand = new RightCommand();
            OpenFolderCommand = new OpenFolderCommand();
            LoadImgCommand = new LoadImgCommand();
            GarmentTypeCommand = new GarmentTypeCommand();
        }

        public LeftCommand LeftCommand
        {
            get;
            internal set;
        }

        public RightCommand RightCommand
        {
            get;
            internal set;
        }

        public OpenFolderCommand OpenFolderCommand
        {
            get;
            internal set;
        }

        public LoadImgCommand LoadImgCommand
        {
            get;
            internal set;
        }

        public GarmentTypeCommand GarmentTypeCommand
        {
            get;
            internal set;
        }
        
    }
}
