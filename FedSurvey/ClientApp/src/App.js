import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { History } from './components/History';
import { QuestionPage } from './components/QuestionPage';
import { DataGroupMerge } from './components/DataGroupMerge';
import { DataGroupCreate } from './components/DataGroupCreate';
import { Analyze } from './components/Analyze';

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
            <Route exact path='/' component={Analyze} />
            <Route path='/questions/:questionId' component={QuestionPage} />
            <Route path='/data-groups/merge' component={DataGroupMerge} />
            <Route path='/data-groups/create' component={DataGroupCreate} />
            <Route path='/history' component={History} />
      </Layout>
    );
  }
}
