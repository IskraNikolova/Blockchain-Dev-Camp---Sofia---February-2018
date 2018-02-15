pragma solidity ^0.4.18;

contract StructProblem{
    
    struct Account {
        string name;
        address addr;
        string email;
    }
    
    Account[] accounts;
    address private owner;
    
    modifier isOwner(){
        require(owner == msg.sender);
        _;
    }
    
    modifier isProperUser(address addr){
        require(addr == msg.sender);
        _;
    }
    
    function StructProblem() public{
        owner = msg.sender;
        
    }
    
    function create(string name, address addr, string email) isProperUser(addr) public{
        Account memory currenAccount;
        currenAccount.name = name;
        currenAccount.addr = addr;
        currenAccount.email = email;
        accounts.push(currenAccount);
    }
    
    function get(uint index) isOwner view public returns(string, address, string){
        Account memory currenAccount = accounts[index];
        return (currenAccount.name, currenAccount.addr, currenAccount.email);
    }
}