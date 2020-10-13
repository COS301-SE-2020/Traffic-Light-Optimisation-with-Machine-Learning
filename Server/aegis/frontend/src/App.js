import React from 'react';
import { BrowserRouter, Switch, Route } from 'react-router-dom';
//import './App.css';
import Login from './component/Login';
import Register from './component/Register';
import Dashboard from './component/Dashboard';
import Admin from './component/Admin';
import Statistics from './component/Statistics';
import 'bootstrap/dist/css/bootstrap.min.css';
import './css/main.css';
function App() {
  return (
    <div className="App">
      {/* <link
        rel="stylesheet"
        href="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css"
        integrity="sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T"
        crossorigin="anonymous"
      /> */}
      <BrowserRouter>
        <div>
          <div className="content">
            <Switch>
              <Route exact path="/" component={Dashboard} />
              <Route exact path="/statistics" component={Statistics} />
              <Route component={Dashboard} />
              {/* <Route exact path="/" component={Login} />
              <Route exact path="/register" component={Register} />
              <Route exact path="/dashboard" component={Dashboard} />
              <Route exact path="/admin" component={Admin} />
              <Route exact path="/statistics" component={Statistics} />
              <Route component={Login} /> */}
            </Switch>
          </div>
        </div>
      </BrowserRouter>
    </div>
  );
}

export default App;
