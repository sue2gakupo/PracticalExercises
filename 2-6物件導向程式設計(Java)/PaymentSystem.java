public class PaymentSystem {
    public static void main(String[] args) {
        
Employee employee1=new HourlyEmployee("Mary","0958624713",200,40);
System.out.println(employee1.toString());
employee1.earnings();
employee1.getTax();
employee1.getPaymentAmount();

System.out.println("----------------------------------------------");

Employee employee2=new SalariedEmployee("Susan", "0932614572", 1500, 0.1, 30000);
System.out.println(employee2.toString());
employee2.earnings();
employee2.getTax();
employee2.getPaymentAmount();

System.out.println("----------------------------------------------");

Employee employee3=new CommissonEmployee("Fay", "0985523364", 20000, 0.2, 30000);
System.out.println(employee3.toString());
employee3.earnings();
employee3.getTax();
employee3.getPaymentAmount();

System.out.println("----------------------------------------------");

Employee employee4=new BasePlusCommissionEmployee("Max", "0974125632", 20000, 0.2, 30000);
System.out.println(employee4.toString());
employee4.earnings();
employee4.getTax();
employee4.getPaymentAmount();






















        
    }
    
}
