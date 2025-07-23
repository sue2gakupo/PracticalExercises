public class SalariedEmployee extends Employee {

    private int grossSales;
    private double commissionRate;
    private int baseSalary;

    SalariedEmployee(String name, String mobile,int grossSales, double commissionRate, int baseSalary) {
        super(name, mobile);
        this.grossSales = grossSales;
        this.commissionRate = commissionRate;
        this.baseSalary = baseSalary;
    }

    public int getGrossSales() {
        return grossSales;
    }
    public void setGrossSales(int grossSales) {
        this.grossSales = grossSales;
    }
    public double getCommissionRate() {
        return commissionRate;
    }
    public void setCommissionRate(double commissionRate) {
        this.commissionRate = commissionRate;
    }
    public int getBaseSalary() {
        return baseSalary;
    }
    public void setBaseSalary(int baseSalary) {
        this.baseSalary = baseSalary;
    }

    @Override
    public String toString(){
        return "Salaried員工基本資料 姓名=" + this.getName() + ", 手機=" + this.getMobile();
    }

    @Override
    public void getTax(){
        double tax  = (this.getBaseSalary()+this.getGrossSales()*this.getCommissionRate())*0.05;
        System.out.println("須扣稅額為:" + tax);
    }

    @Override
    public void earnings(){
        double earnings=this.getBaseSalary()+this.getGrossSales()*this.getCommissionRate();
        System.out.println("未扣稅所得為:" + earnings);
    }

    @Override
    public void getPaymentAmount(){
        double earnings=this.getBaseSalary()+this.getGrossSales()*this.getCommissionRate();
        double tax  = (this.getBaseSalary()+this.getGrossSales()*this.getCommissionRate())*0.05;
        System.out.println("扣稅所得為:" + (earnings-tax));
    }
    
}
 
    

