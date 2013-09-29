using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VintageGarmentDescriber
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region other classes

        public class GarmentImageClass
        {
            public GarmentImageClass(String fileName)
            {
                this.IntImage = new Image();
                BitmapSrc = new BitmapImage();
                BitmapSrc.BeginInit();
                BitmapSrc.UriSource = new Uri(fileName, UriKind.Relative);
                BitmapSrc.CacheOption = BitmapCacheOption.OnLoad;
                BitmapSrc.EndInit();
                IntImage.Source = BitmapSrc;
                IntImage.Stretch = Stretch.Uniform;
            }

            public Image IntImage;
            public BitmapImage BitmapSrc;

        }
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();


            garments = new List<GarmentDescription>();

            garmentUIGroups = new List<Grid>();
            garmentUIGroups.Add(this.TypeControls);
            garmentUIGroups.Add(this.YearControls);
            garmentUIGroups.Add(this.MaterialControls);

            
            loadImgCmd = (LoadImgCommand)((MainViewModel)DataContext).LoadImgCommand;

            ImageIdx = 0;
            IsNextImage = false;
            GarmentDescIdx = -1;

            this.ImgFolder.Text = "D:\\Photos\\Processed\\SmallPhotos\\FF Online\\Clothes\\";
            outputFileName = "descr.txt";
            
        }

#region properties
        String outputFileName;
        
        
        Int32 imageIdx;
        public Int32 ImageIdx
        {
            get
            {
                return imageIdx;
            }
            set
            {
                IsNextImage = imageIdx < value && value >= 0;
                IsPrevImage = imageIdx > value && value >= 0;

                imageIdx = value < 0 ? 0 : value;

                LoadImgCommand cmd = new LoadImgCommand();
                cmd.Execute(this);
                
                this.FileNumber.Content = String.Join(" / ", (this.ImageIdx + 1).ToString(), (this.ImageCount).ToString());
            }
        }

        public Int32 ImageCount;

        public bool IsNextImage;
        public bool IsPrevImage;

        
        List<Grid> garmentUIGroups;

        public Int32 GarmentDescCount { get { return garmentUIGroups.Count; } }

        
        Int32 garmentDescIdx;
        public Int32 GarmentDescIdx
        {
            get
            {
                return garmentDescIdx;
            }
            set
            {
                if (garmentDescIdx > value)
                {
                    this.GarmentIdx = this.prevGarmentIdx;
                }
                else
                {
                    this.prevGarmentIdx = this.GarmentIdx;
                }

                garmentDescIdx = value;
                if (value < 0)
                {
                    garmentDescIdx = ImageIdx == 0 ? 0 : this.garmentUIGroups.Count - 1;
                    --this.ImageIdx;
                }
                else if (this.garmentUIGroups != null && value >= this.garmentUIGroups.Count && this.garmentUIGroups.Count != 0)
                {
                    garmentDescIdx = 0;
                    ++this.ImageIdx;
                }

                AddedProp.Text = "";

                foreach(Grid group in garmentUIGroups)
                {
                    group.Visibility = garmentUIGroups.IndexOf(group) == garmentDescIdx ? Visibility.Visible : Visibility.Collapsed;
                    if (group.Visibility == Visibility.Visible)
                    {
                        this.AddedPropLabel.Content = group.Name.Replace("Controls", "");
                        this.AddedPropNumber.Content = String.Join(" / ", (this.GarmentDescIdx + 1).ToString(), 
                            this.garmentUIGroups.Count.ToString());
                    }
                }
            }
        }

        Int32 prevGarmentIdx;
        Int32 garmentIdx;
        public Int32 GarmentIdx
        {
            get { return garmentIdx; }
            set
            {
                garmentIdx = value < 0 ? 0 : value;
                
                
            }
        }

        LoadImgCommand loadImgCmd;
        public List<GarmentDescription> garments;

        public String ImageFolder {
            get
            {
                return this.ImgFolder.Text;
            }
            set { 
                this.ImgFolder.Text = value; 
            } 
        }

        protected GarmentImageClass garmentImage;
        public GarmentImageClass GarmentImage
        {
            get
            {
                return garmentImage;
            }
            set
            {
                this.sp.Children.Clear();
                this.sp.Children.Add(value.IntImage);
                garmentImage = value;
            }
        }

#endregion

        public Grid GetVisibleGarmentUIGroup()
        {
            if (garmentUIGroups == null)
                return null;

            foreach (Grid group in garmentUIGroups)
            {
                if (group != null && group.Visibility == Visibility.Visible)
                    return group;
            }
            return null;
        }

        private void ImgFolder_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            sp.Children.Clear();
            LoadImgCommand.ClearFileArray();
            loadImgCmd.Execute(this);
        }


        public void AddNewDescr(String text)
        {
            String type = text;
            int lastIdxOfSpace = type.LastIndexOf(' ');
            AddedProp.Text = text.Remove(lastIdxOfSpace, type.Length - lastIdxOfSpace);
            
            if (IsNextImage)
            {
                ++this.GarmentIdx;
                IsNextImage = false;
            }
            else if (IsPrevImage)
            {
                --this.GarmentIdx;
                IsPrevImage = false;
            }

            if (garments.Count < this.GarmentIdx + 1)
            {
                garments.Add(new GarmentDescription());
            }
            GarmentDescription garmentDesc = garments[GarmentIdx];
            garmentDesc.Add(GarmentDescIdx, this.AddedProp.Text);
            Save(System.IO.Path.Combine(this.ImgFolder.Text, this.outputFileName));

            
        }



        public void Save(String filename)
        {
            StreamWriter sw = new StreamWriter(filename, false);

            //// Header
            //int iColCount = garments[0].ColNumber;

            //for (int i = 0; i < iColCount; i++)
            //{
            //    sw.Write(i.ToString());

            //    if (i < iColCount - 1)
            //    {
            //        sw.Write("\x9");
            //    }
            //}
            //sw.Write(sw.NewLine);

            // Body
            for (int i = 0; i < garments.Count; ++i )
            {
                sw.Write(garments[i].ConvertToFileLine());
                sw.Write(sw.NewLine);
            }

            sw.Close();
        }

        
    }

}
