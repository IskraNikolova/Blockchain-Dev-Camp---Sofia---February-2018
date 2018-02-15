pragma solidity ^0.4.18;

contract PreviousInvoker{
    address private prevInvoker;
    
    function getPreviousinvoker() public returns (bool, address){
        address result = prevInvoker;
        prevInvoker = msg.sender;
        return(result != 0x0, result);
    }
}