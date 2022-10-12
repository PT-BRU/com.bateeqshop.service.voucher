using com.bateeqshop.service.voucher.api.Controllers;
using com.bateeqshop.service.voucher.business;
using com.bateeqshop.service.voucher.business.ViewModel;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace com.bateeqshop.service.voucher.Test.VoucherTest
{
    [TestClass]
    public class VoucherTest
    {
        private Mock<VoucherController> _vocController;
        private Mock<ILogger<VoucherController>> _logger;
        private Mock<IService<VoucherVM>> _voucherService;
        private Mock<IHasUserIdService<UserVoucherVM>> _userVoucherService;

        public VoucherTest()
        {
            _vocController = new Mock<VoucherController>();
            _logger = new Mock<ILogger<VoucherController>>();
            _voucherService = new Mock<IService<VoucherVM>>();
            _userVoucherService = new Mock<IHasUserIdService<UserVoucherVM>>();
        }

        private VoucherController GetController()
        {
            var controller = new VoucherController(_voucherService.Object,_userVoucherService.Object,_logger.Object);
            return controller;
        }

        [TestMethod]
        public void FindAsync_Test()
        {
            var controller = GetController();
            var result = controller.FindAsync().Result;
        }

        [TestMethod]
        public void FindAsyncByUserId_Test()
        {
            var controller = GetController();
            var result = controller.FindByUserIdAsync().Result;
        }
    }
}
