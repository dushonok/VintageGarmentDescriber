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
            PrevGarmentTypeCommand = new PrevGarmentTypeCommand();
            NextGarmentTypeCommand = new NextGarmentTypeCommand();
            PrevImageCommand = new PrevImageCommand();
            NextImageCommand = new NextImageCommand();
            OpenFolderCommand = new OpenFolderCommand();
            LoadImgCommand = new LoadImgCommand();
            GarmentTypeCommand = new GarmentTypeCommand();

            ChooseGarmentTypeCommand = new ChooseGarmentTypeCommand();
        }

        public PrevGarmentTypeCommand PrevGarmentTypeCommand
        {
            get;
            internal set;
        }

        public NextGarmentTypeCommand NextGarmentTypeCommand
        {
            get;
            internal set;
        }

        public PrevImageCommand PrevImageCommand
        {
            get;
            internal set;
        }

        public NextImageCommand NextImageCommand
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

        public ChooseGarmentTypeCommand ChooseGarmentTypeCommand
        {
            get;
            internal set;
        }        
        
    }
}
