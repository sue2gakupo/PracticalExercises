public class HourlyEmployee extends Employee {

private int wage;
private int hours;

public int getWage() {
    return wage;
}
public void setWage(int wage) {
    this.wage = wage;
}
public int getHours() {
    return hours;
}
public void setHours(int hours) {
    this.hours = hours;}

public HourlyEmployee(String name, String mobile, int wage, int hours) {
    super(name, mobile);
    this.wage = wage;
    this.hours = hours;
}

@Override
public String toString(){
    return "時薪員工基本資料 姓名=" + this.getName() + ", 手機=" + this.getMobile();
}    

@Override
public void getTax(){
    double tax  = this.getWage()*this.getHours()*0.05;
    System.out.println("須扣稅額為:" + tax);
}

@Override
public void earnings(){
    int earnings=this.getWage()*this.getHours();
    System.out.println("未扣稅所得為:" + earnings);
}

@Override
public void getPaymentAmount(){
    int earnings=this.getWage()*this.getHours();
    double tax  = this.getWage()*this.getHours()*0.05;
    System.out.println("扣稅所得為:" + (earnings-tax));
}



















    
}
