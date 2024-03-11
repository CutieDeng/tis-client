namespace maui0 ;

public record struct SchoolId (string Value); 

public record class User(SchoolId schoolId) { 

    public string? passWord; 
    
        
}