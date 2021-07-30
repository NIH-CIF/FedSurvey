import React, { Component } from 'react';
import { Table } from 'reactstrap';
import { Input, Label } from 'reactstrap';
import { Link } from 'react-router-dom';
import _ from 'lodash';
import { ResultsDataTable } from './ResultsDataTable';
import { Dropdown } from 'bootstrap';

export class Analyze extends Component {
    static displayName = Analyze.name;

    constructor(props) {
        super(props);
        this.state = {
            groupingVariable: 'dataGroupName',
            sortingVariable: 'executionTime',
            filters: {
                'question-ids': [1],
                'possible-response-names': ['Positive']
            },
            questions: [],
            dataGroups: [],
            executions: [],
            possibleResponses: []
        };
    }

    componentDidMount() {
        this.populateDropdownData();
    }

    render() {
        const totalVariables = ['dataGroupName', 'executionTime', 'possibleResponseName', 'questionText'];
        const rowCols = [this.state.groupingVariable, this.state.sortingVariable];
        const notRowCols = totalVariables.filter(v => !rowCols.includes(v));

        const totalExpandedVariables = totalVariables.map(v => {
            if (v === 'dataGroupName') {
                return {
                    tableName: 'dataGroupName',
                    listName: 'dataGroups',
                    displayName: 'Organization',
                    displayValue: 'name',
                    storeValue: 'name',
                    filterKey: 'data-group-names'
                };
            } else if (v === 'executionTime') {
                return {
                    tableName: 'executionTime',
                    listName: 'executions',
                    displayName: 'Year',
                    displayValue: 'key',
                    storeValue: 'key',
                    filterKey: 'execution-times'
                };
            } else if (v === 'possibleResponseName') {
                return {
                    tableName: 'possibleResponseName',
                    listName: 'possibleResponses',
                    displayName: 'Response',
                    displayValue: 'name',
                    storeValue: 'name',
                    filterKey: 'possible-response-names'
                };
            } else if (v === 'questionText') {
                return {
                    tableName: 'questionText',
                    listName: 'questions',
                    displayName: 'Question',
                    displayValue: 'body',
                    storeValue: 'id',
                    filterKey: 'question-ids'
                };
            }
        });
        const dropdownVariables = totalExpandedVariables.filter(tv => notRowCols.includes(tv.tableName));

        return (
            <div>
                <div>
                    <Link to='/'>Home</Link>
                </div>

                <div style={{ display: 'flex', alignItems: 'center' }}>
                    <div>
                        <Label for="acrossVariable" style={{ marginRight: '0.5rem', marginBottom: 0 }}>Across Variable</Label>
                        <Input type="select" name="acrossVariable" id="acrossVariable" onChange={e => { this.updateTableVariable('sortingVariable', totalExpandedVariables.find(dv => dv.tableName === this.state.sortingVariable), totalExpandedVariables.find(dv => dv.tableName === e.target.value), e.target.value) }} value={this.state.sortingVariable} style={{ flexShrink: 2100 }}>
                            {notRowCols.concat(this.state.sortingVariable).filter(v => v !== 'questionText').map(v => (
                                <option value={v} key={v}>{totalExpandedVariables.find(dv => dv.tableName === v).displayName}</option>
                            ))}
                        </Input>
                    </div>

                    <div>
                        <Label for="downVariable" style={{ marginRight: '0.5rem', marginBottom: 0 }}>Down Variable</Label>
                        <Input type="select" name="downVariable" id="downVariable" onChange={e => { this.updateTableVariable('groupingVariable', totalExpandedVariables.find(dv => dv.tableName === this.state.groupingVariable), totalExpandedVariables.find(dv => dv.tableName === e.target.value), e.target.value) }} value={this.state.groupingVariable} style={{ flexShrink: 2100 }}>
                            {notRowCols.concat(this.state.groupingVariable).filter(v => v !== 'executionTime').map(v => (
                                <option value={v} key={v}>{totalExpandedVariables.find(dv => dv.tableName === v).displayName}</option>
                            ))}
                        </Input>
                    </div>
                </div>

                <div style={{ display: 'flex', alignItems: 'center' }}>
                    {dropdownVariables.map(dv => (
                        <div id={dv.tableName}>
                            <Label for={dv.tableName} style={{ marginRight: '0.5rem', marginBottom: 0 }}>{dv.displayName}</Label>
                            <Input type="select" name={dv.tableName} id={dv.tableName} onChange={e => { this.updateFilters(dv, e.target.value) }} value={this.getFilters(dv)} style={{ flexShrink: 2100 }}>
                                {this.state[dv.listName].map(dvi => (
                                    <option value={dvi[dv.storeValue]} key={dv.tableName + dvi[dv.storeValue]}>{dvi[dv.displayValue]}</option>
                                ))}
                            </Input>
                        </div>
                    ))}
                </div>

                <div>
                    <ResultsDataTable
                        filters={this.state.filters}
                        groupingVariable={this.state.groupingVariable}
                        sortingVariable={this.state.sortingVariable}
                    />
                </div>
            </div>
        );

        /*return (
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
                                'data-group-names': ['OFC STRATEGIC COORDINATION'],
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
                                'data-group-names': ['OFC STRATEGIC COORDINATION']
                            }}
                            groupingVariable="questionText"
                            sortingVariable="possibleResponseName"
                        />
                    </div>
                )}
            </div>
        );*/
    }

    // combine translation for next two functions - maybe store it in dropdownVariable
    getFilters(dropdownVariable) {
        if (dropdownVariable.tableName === 'dataGroupName') {
            return this.state.filters['data-group-names'];
        } else if (dropdownVariable.tableName === 'executionTime') {
            return this.state.filters['execution-keys'];
        } else if (dropdownVariable.tableName === 'questionText') {
            return this.state.filters['question-ids'];
        } else if (dropdownVariable.tableName === 'possibleResponseName') {
            return this.state.filters['possible-response-names'];
        }
    }

    updateFilters(dropdownVariable, value) {
        if (dropdownVariable.tableName === 'dataGroupName') {
            this.setState(prevState => ({ filters: { ...prevState.filters, 'data-group-names': [value] } }));
        } else if (dropdownVariable.tableName === 'executionTime') {
            this.setState(prevState => ({ filters: { ...prevState.filters, 'execution-keys': [value] } }));
        } else if (dropdownVariable.tableName === 'questionText') {
            this.setState(prevState => ({ filters: { ...prevState.filters, 'question-ids': [value] } }));
        } else if (dropdownVariable.tableName === 'possibleResponseName') {
            this.setState(prevState => ({ filters: { ...prevState.filters, 'possible-response-names': [value] } }));
        }
    }

    updateTableVariable(variableName, currentDropdownVariable, dropdownVariable, value) {
        const filters = this.state.filters;
        delete filters[dropdownVariable.filterKey];

        this.setState(prevState => ({
            filters,
            [variableName]: value,
        }));
        this.updateFilters(currentDropdownVariable, this.state[currentDropdownVariable.listName][0][currentDropdownVariable.storeValue]);
    }

    async populateDropdownData() {
        // get questions, data groups, executions, possible responses
        // maybe a questions API route for easier processing later
        const response = await Promise.all(
            [
                fetch('api/question-executions'),
                fetch('api/executions'), // later will be an if to be included based on config
                fetch('api/data-groups'),
                fetch('api/possible-responses')
            ]
        );
        const [questionExecutions, executions, dataGroups, possibleResponses] = await Promise.all(
            response.map(r => r.json())
        );

        const questions = Object.entries(_.groupBy(questionExecutions, qe => qe.body)).map(([key, value]) => ({
            body: key,
            id: value[0].questionId
        }));

        this.setState({
            questions,
            executions,
            dataGroups,
            possibleResponses
        });
    }
}
