using GuestBooks.Models;
using HW_Practice.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HW_Practice.Controllers
{
    public class HomeController : Controller
    {
        private readonly GuestBookContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, GuestBookContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _context.Book.Where(b => b.Photo != null).OrderByDescending(b => b.CreateDate).Take(5).ToListAsync();


            return View(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

//���Ȥ@�G�Ш̾��D�D�W���ݨD�A�HCode-First�覡�إ߲ŦX�U�C�ݨD���ҫ����O�BDbContext�θ�Ʈw�A�����]�t�G
//1.	�D���ƪ�(�s��(P.K)�B�D�D�B�o���e�B�Ӥ��B�Ӥ������B�o��H�B�i�K�ɶ�)
//2.	�^�Ф��e��ƪ�(�s��(P.K)�B�^�Ф��e�B�^�ФH�B�^�Юɶ�)
//3.	�D���ƪ�P�^�Ф��e��ƪ��������p�A�@�h�D��i�H���ܦh�h�^�Ф��e�C
//4.	�M�צW�١B��Ʈw�θ�ƪ�W�٥i�ۭq�C
//5.	�U���O�����]�p�۹����ݩʸ�ƫ��A(Data Type)�����ҳW�h�C
//6.	���s�@��l�Ʈɤ��ؤl���(Seed Data)�Y�z���C
//���ȤG�G�г]�p�e�x(�ϥΪ̺�)�\��A�����]�t�G
//1.	�ϥΪ̥i�o��s�峹�A����x�s��D���ƪ�
//2.	�ϥΪ̵o��峹�ɶ���g�D�D�B�o���e�B�o��H���A�ӷӤ��i���i�L�C
//3.	�D�e���e�{�U�Q�פ峹�D�D�A�è̵o��ɶ����s���±ƦC�C
//4.	�I��D�D��i�i�J���Ӥ峹���Q�׵e���A�i�ݨ�D�夺�e�B�Ӥ��Φ^�Ф��e�C
//5.	�ϥΪ̥i�b�峹�Q�׵e���^�иӤ峹�C
//6.	�^�Ф峹�e�{�ɶ��̵o��ɶ����s���±ƦC�C
//���ȤT�G�����X��@��ϥθg��ζK���Ȫ��A�]�A��Ʀs�x���e�B�W�h�Ψϥάy�{�C

