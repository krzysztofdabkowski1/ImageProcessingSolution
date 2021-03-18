using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows.Input;
using System.Windows;
using ImagePresenter.Model;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace ImagePresenter.ViewModel
{
    public class ImagePresenterViewModel : INotifyPropertyChanged
    {
        private ImageProcessorModel model = new ImageProcessorModel();

        private ICommand _LoadImage = null;
        private ICommand _ImageToMainColors = null;
        private IAsyncCommand _ImageToMainColorsAsync = null;
        private ICommand _SaveImage = null;
        private ICommand _ResetImage = null;
        BitmapImage _Image;
        string _imagePath;
        string _elapsedTime;

        public ImagePresenterViewModel()
        {
            _ImageToMainColorsAsync = new AsyncCommand(
                        async () =>
                        {
                            await model.ProcessToMainColorsAsync();
                            _Image = model.GetBitmapImage();
                            _elapsedTime = model.elapsedTime;
                            NotifyPropertyChanged(nameof(Image));
                            NotifyPropertyChanged(nameof(elapsedTime));
                        },
                        (object o) =>
                        {
                            return IsImageLoaded();
                        });

        }
        public event PropertyChangedEventHandler PropertyChanged;

        public BitmapImage Image
        {
            get
            {
                return _Image;
            }
            set
            {
                _Image = value;
                NotifyPropertyChanged(nameof(Image));
            }
        }

        public string elapsedTime
        {
            get
            {
                if(_elapsedTime == null)
                {
                    return "";
                }
                else
                {
                    return _elapsedTime + " ms";
                }            
            }
            set
            {
                _elapsedTime = value;
                NotifyPropertyChanged(nameof(elapsedTime));
            }
        }

        public string imagePath
        {
            get
            {
                return _imagePath;
            }
            set
            {
                _imagePath = value;
                NotifyPropertyChanged(nameof(imagePath));
            }
        }
        public ICommand LoadImage
        {
            get
            {
                if (_LoadImage == null)
                    _LoadImage = new RelayCommand(
                        (object o) =>
                        {
                            if (model.LoadImage())
                            {
                                _Image = model.GetBitmapImage();
                                _imagePath = model.imagePath;
                                _elapsedTime = model.elapsedTime;
                                NotifyPropertyChanged(nameof(Image));
                                NotifyPropertyChanged(nameof(imagePath));
                                NotifyPropertyChanged(nameof(elapsedTime));
                            }

                        });
                return _LoadImage;
            }
        }

        public ICommand ImageToMainColors
        {
            get
            {
                if (_ImageToMainColors == null)
                    _ImageToMainColors = new RelayCommand(
                        (object o) =>
                        {
                            model.ProcessToMainColors();
                            _Image = model.GetBitmapImage();
                            _elapsedTime = model.elapsedTime;
                            NotifyPropertyChanged(nameof(Image));
                            NotifyPropertyChanged(nameof(elapsedTime));
                        },
                        (object o) =>
                        {
                            return IsImageLoaded();
                        });
                return _ImageToMainColors;
            }
        }

        public IAsyncCommand ImageToMainColorsAsync 
        { 
            get
            {
                return _ImageToMainColorsAsync;
            }
        }


        public ICommand Save
        {
            get
            {
                if (_SaveImage == null)
                    _SaveImage = new RelayCommand(
                        (object o) =>
                        {
                            model.Save();
                            NotifyPropertyChanged(nameof(Image));
                        },
                        (object o) =>
                        {
                            return model.isBitmapProcessed;
                        });
                return _SaveImage;
            }
        }

        public ICommand Reset
        {
            get
            {
                if (_ResetImage == null)
                    _ResetImage = new RelayCommand(
                        (object o) =>
                        {
                            model.Reset();
                            _Image = model.GetBitmapImage();
                            _elapsedTime = model.elapsedTime;
                            NotifyPropertyChanged(nameof(Image));
                            NotifyPropertyChanged(nameof(elapsedTime));
                        },
                        (object o) =>
                        {
                            return model.isBitmapProcessed;
                        });
                return _ResetImage;
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private bool IsImageLoaded()
        {
            if(model.GetBitmapImage() == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
