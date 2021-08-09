import React, { Component } from 'react';
import { Route, Switch } from 'react-router';
import { Layout } from './components/Layout';
import { History } from './components/History';
import { QuestionPage } from './components/QuestionPage';
import { DataGroupMerge } from './components/DataGroupMerge';
import { DataGroupCreate } from './components/DataGroupCreate';
import { Admin } from './components/Admin';
import { Analyze } from './components/Analyze';
import { QuestionMerge } from './components/QuestionMerge';

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
        <Layout>
            <Switch>
                <Route exact path='/' component={Analyze} />
                <Route path='/questions/merge' component={QuestionMerge} />
                <Route path='/questions/:questionId' component={QuestionPage} />
                <Route path='/data-groups/merge' component={DataGroupMerge} />
                <Route path='/data-groups/create' component={DataGroupCreate} />
                <Route path='/history' component={History} />
                <Route path='/admin' component={Admin} />
            </Switch>
      </Layout>
    );
  }
}
