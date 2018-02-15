pragma solidity ^0.4.18;

contract SimpleToken{
    mapping(address => uint) public balanceOf;
    
    function SimpleToken(uint256 initialSupply) public{
        balanceOf[msg.sender] = initialSupply;
    }
    
    function transfer(address to, uint256 value) public{
        require(balanceOf[msg.sender] >= value);
        require(balanceOf[to] + value >= balanceOf[to]);
        balanceOf[msg.sender] -= value;
        balanceOf[to] += value;
    }
}