pragma solidity ^0.4.18;


contract ArrayOfFacts{
    
    string[] private facts;
    
    address private contractOwner = msg.sender;


    
    function add(string newFact) public{

       		require(msg.sender == contractOwner);

       		facts.push(newFact);
    
       	}

    
   	function count() view public returns(uint){

       		return facts.length;
    
	}


   
    function getFact(uint index) view public returns(string fact){
       	fact = facts[index];

      	return;
    
 	}

}