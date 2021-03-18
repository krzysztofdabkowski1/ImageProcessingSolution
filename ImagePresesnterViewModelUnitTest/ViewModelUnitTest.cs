using ImagePresenter.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImagePresesnterViewModelUnitTest
{
    [TestClass]
    public class ViewModelUnitTest
    {

        [TestMethod]
        public void TestInitialCommandsActivity()
            
        {
            ImagePresenterViewModel viewModel = new ImagePresenterViewModel();

            Assert.IsFalse(viewModel.ImageToMainColors.CanExecute(null));
            Assert.IsFalse(viewModel.ImageToMainColorsAsync.CanExecute(null));
            Assert.IsFalse(viewModel.Save.CanExecute(null));
            Assert.IsFalse(viewModel.Reset.CanExecute(null));
            Assert.IsTrue(viewModel.LoadImage.CanExecute(null));
        }
    }
}
