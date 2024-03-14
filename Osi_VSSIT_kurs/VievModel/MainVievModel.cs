using DevExpress.Mvvm;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using LiveChartsCore.Defaults;
using ObservablePoint = LiveChartsCore.Defaults.ObservablePoint;
using Osi_VSSIT_kurs.Model;
using Microsoft.Win32;
using System.Drawing;
using Osi_VSSIT_kurs.service;
using DevExpress.Mvvm.Native;
using Data;
using System.Net.Sockets;
using System.Windows;
using TransferServer;
using MenuMetods;

namespace Osi_VSSIT_kurs.VievModel
{
    public class MainVievModel : BaseVievModel
    {
        private ObservableCollection<InputParameters> _parameters;
        private IExcel excel;
        public ObservableCollection<ISeries> LeftGraph { get; set; }
        private ObservableCollection<ObservablePoint> LevelPoints;
        private ObservableCollection<ObservablePoint> LevelLine;

        public ObservableCollection<ISeries> RightGraph { get; set; }
        private ObservableCollection<ObservablePoint> ConcentrationPoints;
        private ObservableCollection<ObservablePoint> ConcentrationLine;
        private Data.ManyParameters _moreParameters;

        private ObservableCollection<object> _methods;
        public ObservableCollection<object> Methods { get => _methods; set => _methods = value; }
        private object? _selectedMethod;
        public object? SelectedMethod { get => _selectedMethod; set => _selectedMethod = value;}




        public ObservableCollection<InputParameters> ManyParameters
        {
            get
            {
                return _parameters;
            }
            set
            {
                if (_parameters != value)
                {
                    _parameters = value;
                    OnPropertyChanged(nameof(ManyParameters));
                }
            }
        }
        
        public Axis[] XExpenditure { get; set; } =
        {
            new Axis
            {
                Name = "Расход, кг/сек",
                NameTextSize = 11,

            }

        };

        public Axis[] YLevel { get; set; } =
        {
            new Axis
            {
                Name = "Уровень, ДМ",
                NameTextSize = 11,
                
            }
            
        };

       
        public Axis[] YConcentration { get; set; } =
        {
            new Axis
            {
                Name = "Концентрация, мг/м*3",
                NameTextSize = 11,
                
            }
        };


        private void PointValue(ObservableCollection<InputParameters> _newPoints)
        {
            foreach (InputParameters point in _newPoints)
            {
                ConcentrationPoints.Add(new ObservablePoint(point.Expenditure, point.Concentration));
                LevelPoints.Add(new ObservablePoint(point.Expenditure, point.Level));
            }

        }
        private void LineValue(Data.ManyParameters _newPoints)
        {
            for (int i = 0; i < _newPoints.Expenditures.Count; i++)
            {
                ConcentrationLine.Add(new ObservablePoint(_newPoints.Expenditures[i], _newPoints.Concentrations[i]));
                LevelLine.Add(new ObservablePoint(_newPoints.Expenditures[i], _newPoints.Levels[i]));
            }
        }

       
        private void TcpConnect()
        {

            if (ManyParameters.Count >= 2)
            {
                TcpClient tcpClient = new TcpClient();
                tcpClient.Connect(Apps.Default.Address, Apps.Default.Port);
                var stream = tcpClient.GetStream();
                TransferS transfers = new TransferS();
                // буфер для входящих данных
                var response = new List<byte>();
                int bytesRead = 10; // для считывания байтов из потока

                // считываем строку в массив байтов
                // при отправке добавляем маркер завершения сообщения - \n
                byte[] data = Encoding.UTF8.GetBytes(transfers.AppEncoder(new Data.ManyParameters(ManyParameters),_selectedMethod, Apps.Default.Step) + '\n');
                // отправляем данные
                stream.WriteAsync(data);
                // считываем данные до конечного символа
                while ((bytesRead = stream.ReadByte()) != '\n')
                {
                    // добавляем в буфер
                    response.Add((byte)bytesRead);
                }
                try
                {
                    var translation = Encoding.UTF8.GetString(response.ToArray());
                    var newParameters = transfers.AppDecoder(translation);
                    stream.WriteAsync(Encoding.UTF8.GetBytes("END\n"));
                    LineValue(newParameters);
                    _moreParameters = newParameters;
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
                response.Clear();
                stream.Close();

            }

        }


        public MainVievModel(IExcel excel)
        {
            this.excel = excel;
            Methods = new ObservableCollection<object>() {
            new Linear(),
            new Parabola()
            };

            LevelPoints = new ObservableCollection<ObservablePoint>();
            ConcentrationPoints = new ObservableCollection<ObservablePoint>();
            LevelLine = new ObservableCollection<ObservablePoint>();
            ConcentrationLine = new ObservableCollection<ObservablePoint>();
            ManyParameters = new ObservableCollection<InputParameters>();
           

            Series = new ObservableCollection<ISeries>
            {
                new LineSeries<ObservablePoint>
                {
                    Values = LevelLine,
                    Stroke = new SolidColorPaint(new(154,205,50), 2),
                    Fill = null,
                    GeometrySize = 0,
                    LineSmoothness = 4,
                    
                },
                new ScatterSeries<ObservablePoint>()
                {
                    Values = LevelPoints,
                    Stroke= new SolidColorPaint(new(120,120,120),2),
                    Fill = default,
                    GeometrySize=4,
                }
            };

            Sseries = new ObservableCollection<ISeries>
            {
                new LineSeries<ObservablePoint>
            {
                Values = ConcentrationLine,
                Stroke = new SolidColorPaint(new(154,205,50), 2),
                Fill = null,
                GeometrySize = 0,
                LineSmoothness = 4,
            },
                new ScatterSeries<ObservablePoint>()
                {
                    Values = ConcentrationPoints,
                    Stroke= new SolidColorPaint(new(120,120,120),2),
                    Fill = default,
                    GeometrySize=4,

                }
            };            
        }
       
        public ObservableCollection<ISeries> Series { get; set; }
        public ObservableCollection<ISeries> Sseries { get; set; }
        
        private void ClearPlot ()
        {
            
            ManyParameters.Clear();
            LevelPoints.Clear();
            ConcentrationPoints.Clear();
            ConcentrationLine.Clear();
            LevelLine.Clear();
        }
        public ICommand DrawApproximation
        {
            get
            {
                return new DelegateCommand(() =>
                {

                    TcpConnect();

                   
                }
                    );
            }

        }

        public ICommand DrawInterpolation
        {
            get
            {
                return new DelegateCommand(() =>
                {

                    PointValue(ManyParameters);


               

                }
                    );
            }

        }

        public ICommand Clear 
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    ClearPlot();
                    
                });
            }
        }

        public ICommand LoadData
        {
            get
            {
                return new DelegateCommand(() =>
                {

                    ManyParameters = excel.ImportFromExcel().ToObservableCollection();
                    if (ManyParameters.Count != 0)
                    {
                        PointValue(ManyParameters);
                    }
                     
                    
                }
                );
            }
        }

       
    }

}
