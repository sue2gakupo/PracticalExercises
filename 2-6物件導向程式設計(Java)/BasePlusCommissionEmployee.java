public class BasePlusCommissionEmployee extends CommissonEmployee {
    
 BasePlusCommissionEmployee(String name, String mobile,int grossSales, double commissionRate, int baseSalary){
    super(name, mobile, grossSales, commissionRate*1.3, baseSalary);

}

@Override
public String toString(){
    return "BasePlusCommisson員工基本資料 姓名=" + this.getName() + ", 手機=" + this.getMobile();
}

}
