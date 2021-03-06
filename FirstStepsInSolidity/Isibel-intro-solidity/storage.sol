pragma solidity ^0.4.18;

contract SimpleStorage{
    uint256 private storedData;
    
    function set(uint256 x) public{
        storedData = x;
    }
    
    function get() constant public returns(uint256){
        return storedData;
    }
}