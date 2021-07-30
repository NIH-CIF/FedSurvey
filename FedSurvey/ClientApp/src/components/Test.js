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
        const CHOSEN_TABLE = 5;

        return (
            <div>
                {CHOSEN_TABLE === 2 && (
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
                )}

                {CHOSEN_TABLE === 3 && (
                    <div>
                        <p>Table 3</p>

                        <ResultsDataTable
                            filters={{
                                'data-group-names': ['OSC TOTAL'],
                                'possible-response-names': ['Positive']
                            }}
                            groupingVariable="questionText"
                            sortingVariable="executionTime"
                        />
                    </div>
                )}

                {CHOSEN_TABLE === 4 && (
                    <div>
                        <p>Table 4</p>

                        <ResultsDataTable
                            filters={{
                                'execution-keys': ['2019'],
                                'possible-response-names': ['Positive']
                            }}
                            groupingVariable="questionText"
                            sortingVariable="dataGroupName"
                        />
                    </div>
                )}

                {CHOSEN_TABLE === 5 && (
                    <div>
                        <p>Table 5</p>

                        <ResultsDataTable
                            filters={{
                                'execution-keys': ['2019'],
                                'question-ids': [1]
                            }}
                            groupingVariable="possibleResponseName"
                            sortingVariable="dataGroupName"
                        />
                    </div>
                )}

                {CHOSEN_TABLE === 6 && (
                    <div>
                        <p>Table 6</p>

                        <ResultsDataTable
                            filters={{
                                'execution-keys': ['2019'],
                                'data-group-names': ['OSC TOTAL']
                            }}
                            groupingVariable="questionText"
                            sortingVariable="possibleResponseName"
                        />
                    </div>
                )}
            </div>
        );
    }
}
