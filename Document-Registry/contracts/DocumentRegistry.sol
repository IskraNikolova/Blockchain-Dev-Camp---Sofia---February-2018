pragma solidity ^0.4.18;

contract DocumentRegistry{
    mapping(string => uint256) documents;
    address contractOwner;
    
    function DocumentRegistry() public{
        contractOwner = msg.sender;
    }
    
    function add(string hash) public returns(uint256 dateAdded){
        require(contractOwner == msg.sender);
        
        dateAdded = block.timestamp;
        documents[hash] = dateAdded;
        
        return dateAdded;
    }
    
    function verify(string hash) view public returns(uint256){
        return documents[hash];
    }
}