import React, { Component } from 'react';
import { Table } from 'reactstrap';
import { Input, Label } from 'reactstrap';
import { Link } from 'react-router-dom';
import { ResultsDataTable } from './ResultsDataTable';

export class Test extends Component {
    static displayName = Test.name;

    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div>
                <p>Table 2</p>

                <ResultsDataTable
                    filters={{
                        'question-ids': [1],
                        'possible-response-names': ['Positive'],
                    }}
                    groupingVariable="dataGroupName"
                    sortingVariable="executionTime"
                />
            </div>
        );
    }
}
