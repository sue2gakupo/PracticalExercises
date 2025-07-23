public class Employee extends Earning implements IPayable, IInvoice {
    private String name;
    private String mobile;
    // 屬性設定

    public Employee(String name, String mobile) {
        this.name = name;
        this.mobile = mobile;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getMobile() {
        return mobile;
    }

    public void setMobile(String mobile) {
        this.mobile = mobile;
    } // Employee類別的建構子

    @Override
    public String toString() {
        return "";
    }
    
    @Override
    public void getTax(){
        
    }

    @Override
    public void earnings() {
                
    }

    @Override
    public void getPaymentAmount() {
      
    }


}
