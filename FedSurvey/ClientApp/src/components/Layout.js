import React, { Component } from 'react';
import { Container } from 'reactstrap';

export class Layout extends Component {
  static displayName = Layout.name;

  render () {
    return (
        <div style={{ height: '100%' }}>
            <Container style={{ height: '100%' }}>
                {this.props.children}
            </Container>
        </div>
    );
  }
}
