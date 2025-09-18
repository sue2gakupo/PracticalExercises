using System.Diagnostics;
using BondleApplication.Access.Data;
using BondleApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BondleApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly BondleDBContext2 _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger,BondleDBContext2 context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

             var result = await (
            from p in _context.Product
            join v in _context.ProductVariations on p.ProductID equals v.ProductID into pv
            from v in pv.DefaultIfEmpty()
            join img in _context.ProductImages on v.VariationID equals img.VariationID into vi
            from img in vi.DefaultIfEmpty()
            where img.ImageUrl != null
            orderby img.ImageID descending
            select new BondleApplication.Models.ViewModel.VisitorProductViewModel
            {
                ProductID = p.ProductID,
                ProductName = p.ProductName,
                Price = p.Price,
                PurchaseCount = p.PurchaseCount,
                VariationID = v != null ? v.VariationID : null,
                VariationName = v != null ? v.VariationName : null,
                Stock = v != null ? v.Stock : 0,
                ImageID = img != null ? img.ImageID : null,
                ImageUrl = img != null ? img.ImageUrl : null,
                ImageCaption = img != null ? img.ImageCaption : null,
                SortOrder = img != null ? img.SortOrder : 0
            })
            .Take(5).ToListAsync();
           
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




///////////////////////////////////////////////////////
//3. ���g�ҫ����e

//3.1 ���}tStudent.cs�ɮ�
//3.2 ���g�bView�W��ܪ���줺�e(��using System.ComponentModel.DataAnnotations)
//3.3 ���g�b���W��������ҳW�h
//    �`�Ϊ����Ҿ� Required�BStringLength�BRegularExpression�BCompare�BEmailAddress�BRange�BDataType
//    Required:�������
//    StringLength:��Ʀr��
//    RegularExpression:��Ʈ榡(���h)
//    Compare:�P�䥦������O�_�۵�
//    EmailAddress:�O�_�OE-mail�榡
//    Range: ����Ҷ񪺽d��
//3.4 �btStudentsController�ɮת�Post Create Action�����g�ˬd�Ǹ��O�_���ƪ��{���X
//3.5 �bCreate View�ɮפ��ɤW�e�{�Ǹ����ƪ����~�T��



///////////////////////////////////////////////////////
//4.�s�@��u���y��tStudent��ƪ�CRUD�\��

//4.1   �إ�MyStudentsController
//4.1.1 �bControllers��Ƨ��W���k����[�J�����
//4.1.2 ��ܡuMVC��� - �ťաv
//4.1.3 ��J�ɦWMyStudentsController.cs
//4.1.4 ���g�إ�DbContext���󪺵{��

//4.2   �إߦP�B���檺Index Action
//4.2.1 ���gIndex Action�{���X
//4.2.2 �إ�Index View
//4.2.3 �bIndex Action�����k����s�W�˵�����ܡuRazor�˵��v�����U�u�[�J�v�s
//4.2.4 �b��ܤ�����]�w�p�U
//      �˵��W��: Index
//      �d��:List
//      �ҫ����O: tStudent(MyModel_DBFirst.Models)
//      ��Ƥ��e���O: dbStudentsContext(MyModel_DBFirst.Models)
//      ���Ŀ� �إߦ������˵�
//      ���Ŀ� �Ѧҫ��O�X�{���w
//      �Ŀ� �ϥΪ����t�m��
//4.2.5 ����Index View����
//4.2.6 �ק虜���W����r�A����Details���W�쵲
//      ���i�̦ۤv���ߦn�ק�View����ܡ�


//4.3   �إߦP�B���檺Create Action
//4.3.1 ���gCreate Action�{���X(�ݦ����Create Action)
//4.3.2 �إ�Create View
//4.3.3 �bCreate Action�����k����s�W�˵�����ܡuRazor�˵��v�����U�u�[�J�v�s
//4.3.4 �b��ܤ�����]�w�p�U
//      �˵��W��: Create
//      �d��:Create
//      �ҫ����O: tStudent(MyModel_DBFirst.Models)
//      ��Ƥ��e���O: dbStudentsContext(MyModel_DBFirst.Models)
//      ���Ŀ� �إߦ������˵�
//      �Ŀ� �Ѧҫ��O�X�{���w
//      �Ŀ� �ϥΪ����t�m��
//4.3.5 ����Create�\�����
//      ���i�̦ۤv���ߦn�ק�View����ܡ�

//4.3.6 �[�J�ˬd�D��O�_���Ъ��{��
//4.3.7 �[�JToken���Ҽ���
//  ��Token���Ҫ��\�άO����CSRF�����A����洣��ɯ�����ҽШD���X�k�ʡ�


//4.4   �إߦP�B���檺Edit Action
//4.4.1 ���gEdit Action�{���X(�ݦ����Edit Action)
//4.4.2 �إ�Edit View
//4.4.3 �bEdit Action�����k����s�W�˵�����ܡuRazor�˵��v�����U�u�[�J�v�s
//4.4.4 �b��ܤ�����]�w�p�U
//      �˵��W��: Edit
//      �d��:Edit
//      �ҫ����O: tStudent(MyModel_DBFirst.Models)
//      ��Ƥ��e���O: dbStudentsContext(MyModel_DBFirst.Models)
//      ���Ŀ� �إߦ������˵�
//      �Ŀ� �Ѧҫ��O�X�{���w
//      �Ŀ� �ϥΪ����t�m��
//4.4.5 ����Edit�\�����
//      ���i�̦ۤv���ߦn�ק�View����ܡ�


//4.5   �إߦP�B���檺Delete Action
//4.5.1 ���gDelete Action�{���X
//4.5.2 �NIndex View��Delete�אּForm�A�HPost�覡�e�X
//4.5.3 �NDelete Action�אּPost�覡
//4.5.4 ����Delete�\�����
//���ɥR������
//�o�ؼg�k�Τ���Delete View�A�]���i�H��Delete.cshtml�R��
//Delete�����s�Y�ϥζW�쵲�A�ϥΪ̱N�i�����burl���ѼƴN��R�����



//�d�ұ��ҡG�ǥͭn�h�X��t��ơA�ҥH��Ʈw�ݭn�ק�A�إߤ@�Ӭ�t��ƪ�ûPtStudent��ƪ����p
//5. ��Ʈw�ק�
//���ѩ�DB First�O�H�ϦV�u�{�Q�θ�Ʈw�g�����{���X�A�]���b��Ʈw���p�T�ܰʮɡA�h������ʼ��g�ҫ����e��

//5.1   �btStudent��ƪ��W�[�@�����
//5.1.1 �bSSMS������U�CDDL���O�X�H�ק�tStudent��ƪ�ΡA�W�[�@��DeptID���
//alter table tStudent
//	    add DeptID varchar(2) not null default '01'
//  go
//5.1.2 �btStudent Class���W�[�@���ݩ� public string DeptID { get; set; }
//5.1.3 �����p�ק�View
//5.1.4 �������


//5.2   �bdbStudents��Ʈw���W�[��ƪ�
//5.2.1 �bSSMS������U�CDDL���O�X�H�إ�Department��ƪ�λPtStudnet�����p
//////////////////////////////////////////////////////////
//create table Department(
//    DeptID varchar(2) primary key,
//    DeptName nvarchar(30) not null
//)
//go

//insert into Department values('01','��u�t'),('02', '��ިt'),('03', '�u�ިt')
//go

//alter table tStudent
//	add foreign key(DeptID) references Department(DeptID)
//go
//////////////////////////////////////////////////////////

//5.2.2 �bModels��Ƨ����s�WDepartment Class(Models��Ƨ��W���k����[�J�����O)�A���e�p�U
//public class Department
//{
//    [Key]
//    public string DeptID { get; set; }
//    public string DeptName { get; set; } = null!
//    public List<tStudent>? tStudents { get; set; }
//}
//5.2.3 �ק�tStudent Class�H�إ߻PDepartment�����p�A���e�p�U
//[ForeignKey("Department")]
//public string DeptID { get; set; } = null!
//public virtual Department? Department { get; set; }

//5.2.4 �bdbStudentsContext���[�JDepartment��DbSet

//���ɥR������
//���Y��Ʈw���ܰʴT�׸��j�A�h��ĳ���s����Scaffold - DbContext���O���ؾ�Ӽҫ���
//�����L�Y�HScaffold - DbContext���s�إ߼ҫ��A�|�N��DbContext�ΦU��Class���ܦ^��l���{���X�A���e�ۤv���g�������|����������
//�����Controller��View�ӻ��A�Y���Q���sScaffold CRUD�祲���@�Ӥ@�Ӥ�ʭק


//5.3   ���s�s�@�۰ʥͦ���tStudent��ƪ�CRUD�\��
//5.3.1 �NsControler���W�٧אּtStudents2Controler
//5.3.2 �bControllers��Ƨ��W���k����[�J�����
//5.3.3 ��ܡu�ϥ�EntityFramework�����˵���MVC����v�����U�u�[�J�v�s
//5.3.4 �b��ܤ�����]�w�p�U
//      �ҫ����O: tStudent(MyModel_DBFirst.Models)
//      ��Ƥ��e���O: dbStudentsContext(MyModel_DBFirst.Models)
//      �Ŀ� �ӥ��˵�
//      �Ŀ� �Ѧҫ��O�X�{���w
//      �Ŀ� �ϥΪ����t�m��
//      ����W�٨�tStudents2Controller
//      ���U�u�s�W�v�s
//5.3.5 �Ѧ�2.2.1�ק�إ�DbContext���󪺵{��
//5.3.6 ����



//5.4   �ק�Index View����Department�����

//5.5   ��ʭק�MyStudentsController�ά�����View
//5.5.1 �ק� Index Action
//5.5.2 �ק� Index View
//5.5.3 �ק� Create Action
//5.5.4 �ק� Create View
//5.5.5 �ק� Edit Action
//5.5.6 �ק� Edit View


//5.6 �s�@�۰ʥͦ���Department��ƪ�CRUD�\��
//5.6.1 �bControllers��Ƨ��W���k����[�J�����
//5.6.2 ��ܡu�ϥ�EntityFramework�����˵���MVC����v�����U�u�[�J�v�s
//5.6.3 �b��ܤ�����]�w�p�U
//      �ҫ����O: Department(MyModel_DBFirst.Models)
//      ��Ƥ��e���O: dbStudentsContext(MyModel_DBFirst.Models)
//      �Ŀ� �ӥ��˵�
//      �Ŀ� �Ѧҫ��O�X�{���w
//      �Ŀ� �ϥΪ����t�m��
//      ����W�٨ϥιw�]�Y�i(DepartmentsController)
//      ���U�u�s�W�v�s
//5.6.4 �Ѧ�2 .2.1�ק�إ�DbContext���󪺵{��
//5.6.5 ����
//5.6.6 �s��Department �ҫ�
//5.6.7 �̻ݨD�ק�β���Department View��DepartmentsController Action
//5.6.8 �bDepartmentsController Create Action���[�J�ˬd��t�N�X�O�_���Ъ��{��


//5.7   �s��Layout���A�[�J�u��t�޲z�v�Ρu�ǥ͸�ƺ޲z�v



//5.8   �ϥΪ̤������B�Χޥ�-View Model
//5.8.1 �إ�ViewModels��Ƨ�(�M�פW���k����[�J���s�W��Ƨ�)
//5.8.2 �bViewModels��Ƨ��s�WVMtStudent���O(�k����[�J�����O����JVMtStudent.cs�����U�u�s�W�v�s)����Model
//      (�o��Model�u��View�ƪ��Φ]���٧@ViewModel)
//5.8.3 ���gVMtStudent���O
//5.8.4 ���gMyStudnetsController�̷s��IndexViewModel Action
//5.8.5 �s�@IndexViewModel View�A�õ��ݭn�ק�IndexViewModel View���ƪ��覡
//5.8.6 ����
//5.8.7 �ק�IndexViewModel Action�A�N��t�N�X�ѰѼƶǤJ�A�����z�����
//���ɥR������
//View Model�����O�M��View�]�p��Model�A�D�n�Ω�View���e�{�����ҳW�h(�ӷ~�޿�)




//5.9   ���������A�O�d
//������:�ثe���槹�s�W�έק�\��A�ɦ^Index Action�ɬҷ|�e�{�u��u�t�v���ǥ͸�ơ�
//���o�O�]��Index Action�Y�S���Ѽƫh�|�w�]�ϥ�deptid="01"����ơA�y���y�{�W���p���D��
//���]���ݭn�ק�Create�BEdit�PDelete��Action��View�i��Ѽƶǻ��A�H�O�d������쥻�����A��
//5.9.1 �ק�IndexViewModel View�WCreate���W�쵲�i��Ѽƶǻ�
//5.9.2 �ק�Get Create Action�i��Ѽƶǻ�
//5.9.3 �ק�Post Create Action�i��Ѽƶǻ�
//5.9.4 �ק�Get Edit Action�i��Ѽƶǻ�
//5.9.5 �ק�Post Edit Action�i��Ѽƶǻ�
//5.9.6 �ק�Post Delete Action�i��Ѽƶǻ�
//5.9.7 ����
//���ɥR������
//�u�n�ॿ�T���ǻ��ѼƨӫO�d���A�ASelectList����|�۰� Mapping���T��option



//6. �N���o��Ƨאּ�̿�`�J(Dependency Injection)���g�k

//6.1   ��Ʈw�s�u�r�ꪺ��g
//      ���ثe�ڭ̱N��Ʈw���s�u�r��g�bDbContext�����O��(dbStudentsContext.cs)���A�o�O�@�ظ����n���g�k��
//6.1.1 �N�s�u�r��g�bappsettings.json�ɤ�
//6.1.2 �NdbStudentsContext���Ҽg���s�u�r����ѱ�
//6.1.3 �NdbStudentsContext���Ҽg���ūغc�l���ѱ�(�]�i�d�ۥu�O�Τ���)
//6.1.4 �bProgram.cs�[�J�ϥ�appsettings.json�����s�u�r��{���X(�o�q�����g�kvar builder�o��᭱)


//6.2   �ץ�tStudents2Controller�إ�DbContext���󪺼g�k
//6.2.1 �NtStudents2Controller�إ�DbContext���󪺵{������
//6.2.2 �N�B�JtStudents2Controller�ҵ��ѱ����{����������(�o�̪��g�k�Oscaffold�w�]���̿�`�J�g�k)
//6.2.3 �ѷ�tStudents2Controller�A�ק�MyStudentsController��DepartmentController���إ�DbContext���󪺵{�����̿�`�J�g�k

//���ɥR������
//�̿�`�J(Dependency Injection)�O����ɦV�{���]�p���g�ɪ��@�اޥ�
//��Q�α������(IoC)�������A�Nnew�D�ʫإߪ��ت��g�k���ର�Q�ʦa��������
//���󪺥ͩR�g�������v�h�bDI Container��W�Ӥ��O�ϥΪ��󪺵{��
//�o�Ӽg�k���u�I�O�Ѱ����󤧶������X�A���Q��{�����@�B�椸���յ�