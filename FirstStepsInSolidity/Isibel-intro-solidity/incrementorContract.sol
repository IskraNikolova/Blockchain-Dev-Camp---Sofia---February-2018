pragma solidity ^0.4.18;

contract IncrementorContract{
    uint256 private valueToBeIncremented;

    function get() public view returns(uint256){
        return valueToBeIncremented;
    }
    
    function increment(uint256 delta) public{
        valueToBeIncremented += delta;
    }
}