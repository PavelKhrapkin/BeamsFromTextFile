using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using Tekla.Structures.Model;
using T3D = Tekla.Structures.Geometry3d;

namespace BeamsFromTextFile
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Model Model;
        public MainWindow()
        {
            InitializeComponent();
            try { Model = new Model(); }
            catch { MessageBox.Show("Tekla Model not connected"); Environment.Exit(0); }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string FileName = "BeamToTextFile.txt";
            string FinalFileName = System.IO.Path.Combine(Model.GetInfo().ModelPath, FileName);
            using (StreamReader FileReader = new StreamReader(FinalFileName))
            {
                //      while (FileReader.EndOfStream)
                string DataLine;
                while((DataLine = FileReader.ReadLine()) != null)
                {
                    // Read each line from the file
     //               string DataLine = FileReader.ReadLine();
                    string[] DataArray = Regex.Split(DataLine, ", ");
                    // GUID, PROFILE, MATERIAL, CLASS, SX,SY,SZ, EX,EY,EZ
                    string FromFileGUID = DataArray[0];
                    string FromFileProfile = DataArray[1];
                    string FromFileMaterial = DataArray[2];
                    string FromFileClass = DataArray[3];
                    double FromFileSX = double.Parse(DataArray[4]);
                    double FromFileSY = double.Parse(DataArray[5]);
                    double FromFileSZ = double.Parse(DataArray[6]);
                    double FromFileEX = double.Parse(DataArray[7]);
                    double FromFileEY = double.Parse(DataArray[8]);
                    double FromFileEZ = double.Parse(DataArray[9]);

                    // Check if the object exists in the model
                    Beam ThisBeam = Model.SelectModelObject(Model.GetIdentifierByGUID(FromFileGUID)) as Beam;
                    if(ThisBeam != null)
                    {
                        ThisBeam.Profile.ProfileString = FromFileProfile;
                        ThisBeam.Material.MaterialString = FromFileMaterial;
                        ThisBeam.Class = FromFileClass;
                        ThisBeam.StartPoint = new T3D.Point(FromFileSX, FromFileSY, FromFileSZ);
                        ThisBeam.EndPoint = new T3D.Point(FromFileEX, FromFileEY, FromFileEZ);
                        ThisBeam.Modify();
                    }
                    else
                    {
                        ThisBeam.Profile.ProfileString = FromFileProfile;
                        ThisBeam.Material.MaterialString = FromFileMaterial;
                        ThisBeam.Class = FromFileClass;
                        ThisBeam.StartPoint = new T3D.Point(FromFileSX, FromFileSY, FromFileSZ);
                        ThisBeam.EndPoint = new T3D.Point(FromFileEX, FromFileEY, FromFileEZ);
                        ThisBeam.Modify();
                    }
                }
                Model.CommitChanges();
                Tekla.Structures.Model.Operations.Operation.DisplayPrompt("Beams Modified or Created.");
            }
        }
    }
}