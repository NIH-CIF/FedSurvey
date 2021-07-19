import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { QuestionPage } from './components/QuestionPage';
import { DataGroupMerge } from './components/DataGroupMerge';

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
            <Route exact path='/' component={Home} />
            <Route path='/questions/:questionId' component={QuestionPage} />
            <Route path='/data-groups/merge' component={DataGroupMerge} />
      </Layout>
    );
  }
}
