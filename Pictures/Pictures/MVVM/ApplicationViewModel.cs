using System.ComponentModel;
using System.Collections.ObjectModel;
using Pictures.MVVM.Commands;
using System.Linq;
using Microsoft.Win32;
using System.Runtime.CompilerServices;

namespace Pictures.MVVM
{
    /// <summary>
    /// ViewModel для создания связи с View
    /// </summary>
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Путь до выбранной картинки
        /// </summary>
        private Path _selectedImagePath;
        /// <summary>
        /// Величина в значение которой увеличивается/уменьшается размер картинки
        /// </summary>
        private double _scaleFactor = 1.0;
        /// <summary>
        /// Команда для увеличения изображения
        /// </summary>
        private UpdateCommand _increaseImageCommand;
        /// <summary>
        /// Команда для уменьшения изображения
        /// </summary>
        private UpdateCommand _decreaseImageCommand;
        /// <summary>
        /// Команда для добавления
        /// </summary>
        private UpdateCommand _addCommand;
        /// <summary>
        /// Команда для удаления
        /// </summary>
        private UpdateCommand _removeCommand;
        /// <summary>
        /// Команда для смены выделенного элемента
        /// </summary>
        private UpdateCommand _selectedChangeCommand;
        /// <summary>
        /// Пути до всех загруженных файлов
        /// </summary>
        public ObservableCollection<Path> PathsToImages { get; private set; } = new ObservableCollection<Path>();

        /// <summary>
        /// Команда для уменьшения изображения
        /// </summary>
        public UpdateCommand DecreaseImageCommand
        {
            get
            {
                return _decreaseImageCommand ??
                  (_decreaseImageCommand = new UpdateCommand(obj =>
                  {
                      ScaleFactor /= 1.2;
                  }));
            }
        }
        /// <summary>
        /// Команда для увеличения изображения
        /// </summary>
        public UpdateCommand IncreaseImageCommand
        {
            get
            {
                return _increaseImageCommand ??
                  (_increaseImageCommand = new UpdateCommand(obj =>
                  {
                      ScaleFactor *= 1.2;
                  }));
            }
        }
        /// <summary>
        /// Величина в значение которой увеличивается/уменьшается размер картинки
        /// </summary>
        public double ScaleFactor
        {
            get { return _scaleFactor; }
            set 
            {
                _scaleFactor = value;
                OnPropertyChanged(nameof(ScaleFactor));
            }
        }

        /// <summary>
        /// Команда добавления
        /// </summary>
        public UpdateCommand AddCommand
        {
            get
            {
                return _addCommand ??
                  (_addCommand = new UpdateCommand(obj =>
                  {
                      var saveFileDialog = new OpenFileDialog();
                      saveFileDialog.Filter = "Png Image (.png)|*.png|Bitmap Image (.bmp)|*.bmp|Gif Image (.gif)|*.gif|JPEG Image (.jpeg)|*.jpeg|Tiff Image (.tiff)|*.tiff|Wmf Image (.wmf)|*.wmf";
                      if (saveFileDialog.ShowDialog() == true)
                      {
                          SelectedImagePath = new Path() { PathToImage = saveFileDialog.FileName };
                          PathsToImages.Insert(0, SelectedImagePath);
                      }
                  }));
            }
        }

        /// <summary>
        /// Команда для удаления
        /// </summary>
        public UpdateCommand RemoveCommand
        {
            get
            {
                return _removeCommand ??
                  (_removeCommand = new UpdateCommand(obj =>
                  {
                      PathsToImages.Remove(SelectedImagePath);
                      SelectedImagePath = PathsToImages.FirstOrDefault();
                  },
                 (obj) => SelectedImagePath != null));
            }
        }

        /// <summary>
        /// Команда для смены выделенного элемента
        /// </summary>
        public UpdateCommand SelectedChangeCommand
        {
            get
            {
                return _selectedChangeCommand ??
                  (_selectedChangeCommand = new UpdateCommand(obj =>
                  {
                      SelectedImagePath = PathsToImages.FirstOrDefault(_ => _.PathToImage == (obj as string));
                  }));
            }
        }

        /// <summary>
        /// Выбранный путь
        /// </summary>
        public Path SelectedImagePath
        {
            get { return _selectedImagePath; }
            set
            {
                _selectedImagePath = value;
                OnPropertyChanged(nameof(SelectedImagePath));
            }
        }


        /// <summary>
        /// Привязка к событию для обновления
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
