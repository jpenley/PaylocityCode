import './App.css';
import { useState, useEffect } from "react";
import { useForm } from "react-hook-form";

function App() {
  const [inputList, setInputList] = useState([]);
  const [estimate, setEstimate] = useState();
  const { register, handleSubmit } = useForm();
  
  const onSubmit = form => {
    if(!form.dependents) form.dependents = [];
    let requestOptions = {
      method: "POST",
      headers: {"Content-Type": "application/json"},
      body: JSON.stringify(form)
    };
    fetch("https://localhost:5001/estimate", requestOptions)
      .then(res => res.json())
      .then(json => {
        let totalCost = 0;
        
        json.employee.adjustedCost = json.employee.baseCost - json.employee.discounts.reduce( function(a,b){return a+b["amount"];},0);
        
        totalCost += json.employee.adjustedCost;
        
        if(json.dependents){
          json.dependents.forEach(dependent => {
            dependent.adjustedCost = dependent.baseCost - dependent.discounts.reduce( function(a,b){return a+b["amount"];},0);
            totalCost += dependent.adjustedCost;
          });
        }
        
        json.totalCost = totalCost;
        
        console.log(json);
        setEstimate(json);
      })
  }

  const handleRemoveClick = index => {
    const list = [...inputList];
    list.splice(index, 1);
    console.log(list);
    setInputList(list);
  };

  const handleAddClick = index => {
    setInputList([...inputList, {firstName: "", lastName: ""}]);
  }

  return (
    <div className="App">
      <div className="Employee">
        <span>Employee:</span>
        <input type="text" placeholder="First name" name="employee.firstName" ref={register} />
        <input type="text" placeholder="Last name" name="employee.lastName" ref={register} />
      </div>
      <div>
        {inputList.map((x,i) => {
          return (
            <div key={"d"+i} className="Dependents">
              <span key={"l"+i}>Dependent:</span>
              <input key={"i"+i} type="text" placeholder="First name" name={"dependents["+i+"].firstName"} ref={register} />
              <input key={"ii"+i} type="text" placeholder="Last name" name={"dependents["+i+"].lastName"} ref={register} />
              {<button key={"b"+i} onClick={() => handleRemoveClick(i)}>X</button>}
            </div>
          );
        })}
      </div>
      <span className="bottomButton">
        <button onClick={handleAddClick}>Add Dependent</button>
        <button onClick={handleSubmit(onSubmit)}>Submit</button>
      </span>
      {estimate &&
        <div className="Estimate">
          <div>Employee: {estimate.employee.individual.firstName} {estimate.employee.individual.lastName}</div>
          <div className="colRight">Annual cost: ${estimate.employee.baseCost.toFixed(2)}</div>
          {estimate.employee.discounts.length > 0 &&
            <div>
              <span className="colCenter">Discounts:</span>
              {estimate.employee.discounts.map((discount) => {
                return (
                  <span className="colRight" key={discount.description}>{discount.description}: ${discount.amount.toFixed(2)}</span>
                );
              })}
              <span className="colRight">Adjusted annual cost: ${estimate.employee.adjustedCost.toFixed(2)}</span>
            </div>
          }
          {estimate.dependents && estimate.dependents.map((dependent) => {
            return (
              <div>
                <div>
                  <span className="Dependent">Dependent: {dependent.individual.firstName} {dependent.individual.lastName}</span>
                  <span className="colRight">Annual cost: ${dependent.baseCost.toFixed(2)}</span>
                </div>
                {dependent.discounts.length > 0 &&
                  <div>
                    <span className="colCenter">Discounts:</span>
                    {dependent.discounts.map((discount) => {
                      return (
                        <span className="colRight">{discount.description}: ${discount.amount.toFixed(2)}</span>
                      );
                    })}
                    <span className="colRight">Adjusted annual cost: ${dependent.adjustedCost.toFixed(2)}</span>
                  </div>
                }
              </div>
            );
          })}
          <span className="colRight">Total annual cost: ${estimate.totalCost.toFixed(2)}</span>
          <span className="colRight">Cost per pay period: ${(estimate.totalCost/estimate.employee.individual.payPeriods).toFixed(2)}</span>
        </div>
      }
    </div>
  )
}

export default App;
