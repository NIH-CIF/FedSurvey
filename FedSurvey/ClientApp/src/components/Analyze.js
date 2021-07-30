import React, { Component } from 'react';
import { Table } from 'reactstrap';
import { Input, Label } from 'reactstrap';
import { Link } from 'react-router-dom';
import _ from 'lodash';
import { ResultsDataTable } from './ResultsDataTable';
import { Dropdown } from 'bootstrap';
import Select from 'react-select';

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
            questionExecutions: [],
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

        // need to redo naming thru
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
                    filterKey: 'execution-keys'
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
                    listName: 'questionExecutions',
                    displayName: 'Question',
                    displayValue: 'body',
                    storeValue: 'questionId',
                    filterKey: 'question-ids'
                };
            }
        });
        const dropdownFilterVariables = totalExpandedVariables.filter(tv => notRowCols.includes(tv.tableName));
        const dropdownSelectVariables = totalExpandedVariables.filter(tv => rowCols.includes(tv.tableName));

        return (
            <div>
                <div>
                    <Link to='/'>Home</Link>
                </div>

                <div style={{ display: 'flex', alignItems: 'center' }}>
                    <div style={{ flex: 1 }}>
                        <Label for="acrossVariable">
                            Across Variable
                        </Label>
                        <Input
                            type="select"
                            name="acrossVariable"
                            id="acrossVariable"
                            onChange={e => this.updateTableVariable(
                                'sortingVariable',
                                totalExpandedVariables.find(dv => dv.tableName === this.state.sortingVariable),
                                totalExpandedVariables.find(dv => dv.tableName === e.target.value),
                                e.target.value
                            )}
                            value={this.state.sortingVariable}
                        >
                            {notRowCols
                                .concat(this.state.sortingVariable)
                                .filter(v => v !== 'questionText')
                                .map(v => (
                                <option value={v} key={v}>{totalExpandedVariables.find(dv => dv.tableName === v).displayName}</option>
                            ))}
                        </Input>
                    </div>

                    <div style={{ flex: 1 }}>
                        <Label for="downVariable">
                            Down Variable
                        </Label>
                        <Input
                            type="select"
                            name="downVariable"
                            id="downVariable"
                            onChange={e => this.updateTableVariable(
                                'groupingVariable',
                                totalExpandedVariables.find(dv => dv.tableName === this.state.groupingVariable),
                                totalExpandedVariables.find(dv => dv.tableName === e.target.value),
                                e.target.value
                            )}
                            value={this.state.groupingVariable}
                        >
                            {notRowCols
                                .concat(this.state.groupingVariable)
                                .filter(v => v !== 'executionTime')
                                .map(v => (
                                <option value={v} key={v}>{totalExpandedVariables.find(dv => dv.tableName === v).displayName}</option>
                            ))}
                        </Input>
                    </div>
                </div>

                <div style={{ display: 'flex', alignItems: 'center' }}>
                    {dropdownFilterVariables.map(dv => (
                        <div key={'div' + dv.tableName} style={{ flex: 1 }}>
                            <Label for={dv.tableName}>
                                {dv.displayName}
                            </Label>
                            <Select
                                name={dv.tableName}
                                id={dv.tableName}
                                onChange={val => this.updateFilters(dv, val)}
                                value={this.getFiltersForSelect(dv)}
                                options={this.state[dv.listName].map(item => ({ label: item[dv.displayValue], value: item[dv.storeValue] }))}
                            />
                        </div>
                    ))}
                </div>

                <div style={{ display: 'flex', alignItems: 'center' }}>
                    {dropdownSelectVariables.map(dv => (
                        <div key={'div' + dv.tableName} style={{ flex: 1 }}>
                            <Label for={dv.tableName}>
                                {dv.displayName}
                            </Label>
                            <Select
                                name={dv.tableName}
                                isMulti
                                id={dv.tableName}
                                onChange={val => this.updateFilters(dv, val)}
                                value={this.getFiltersForSelect(dv)}
                                options={this.state[dv.listName].map(item => ({label: item[dv.displayValue], value: item[dv.storeValue]}))}
                            />
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
    }

    getFilters(dropdownFilterVariable) {
        return this.state.filters[dropdownFilterVariable.filterKey] ? this.state.filters[dropdownFilterVariable.filterKey] : null;
    }

    getFiltersForSelect(dropdownFilterVariable) {
        return this.state[dropdownFilterVariable.listName].map(item => ({ label: item[dropdownFilterVariable.displayValue], value: item[dropdownFilterVariable.storeValue] })).filter(o => this.getFilters(dropdownFilterVariable)?.includes(o.value));
    }

    updateFilters(dropdownFilterVariable, value) {
        const setValue = Array.isArray(value) ? value.map(v => v.value) : [value.value];

        this.setState(prevState => ({ filters: { ...prevState.filters, [dropdownFilterVariable.filterKey]: setValue } }));
    }

    updateTableVariable(variableName, currentDropdownVariable, dropdownFilterVariable, value) {
        this.setState(prevState => ({
            filters: {
                ...prevState.filters,
                [dropdownFilterVariable.filterKey]: []
            },
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

        const groupedQuestionExecutions = Object.entries(_.groupBy(questionExecutions, qe => qe.body)).map(([key, value]) => ({
            body: key,
            questionId: value[0].questionId,
            id: value[0].id
        }));

        this.setState({
            questionExecutions: groupedQuestionExecutions,
            executions,
            dataGroups,
            possibleResponses
        });
    }
}
