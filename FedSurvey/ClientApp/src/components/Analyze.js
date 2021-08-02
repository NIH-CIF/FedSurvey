import React, { Component } from 'react';
import { Label } from 'reactstrap';
import { Link } from 'react-router-dom';
import _ from 'lodash';
import { ResultsDataTable } from './ResultsDataTable';
import Select from 'react-select';

export class Analyze extends Component {
    static displayName = Analyze.name;

    constructor(props) {
        super(props);
        this.state = {
            groupingVariable: null,
            sortingVariable: null,
            filters: {},
            questionExecutions: [],
            dataGroups: [],
            executions: [],
            possibleResponses: [],
            setupComplete: false
        };
    }

    componentDidMount() {
        this.populateDropdownData();
    }

    instructionalRow() {
        const downAcrossSelected = this.state.groupingVariable !== null && this.state.sortingVariable !== null;
        const filtersSelected = this.getNonEmptyFiltersCount() > 1;

        return !this.state.setupComplete && (
            <span>
                {(!downAcrossSelected || !filtersSelected) && (
                    <b style={{ marginRight: 4 }}>Setup:</b>
                )}

                {downAcrossSelected && !filtersSelected && (
                    `Now select what values you will lock the other variables into for this table.
                    You will be looking at all or some instances of your down variable and across variable, but only one instance of these variables.
                    After selecting this, the last two select boxes will open, where you can select multiple instances to view part of the larger table.
                    You will also be free to change any variable across the six boxes.`
                )}
                {!downAcrossSelected && !filtersSelected && (
                    `Select what variable will go down your table and across your table.
                    These variables will generally be what you are comparing.
                    Note that Year can only be an across variable and Question can only be a down variable.`
                )}
            </span>
        );
    }

    dropdownRow(dropdownVariables) {
        return (
            <div style={{ display: 'flex', alignItems: 'center' }}>
                {dropdownVariables.map(dv => {
                    const isMulti = this.state.groupingVariable === dv.tableName || this.state.sortingVariable === dv.tableName;
                    const isDisabled = isMulti ? this.getNonEmptyFiltersCount() < 2 : (this.state.groupingVariable === null || this.state.sortingVariable === null);

                    return (
                        <div key={'div' + dv.tableName} style={{ flex: 1 }}>
                            <Label for={dv.tableName}>
                                {dv.displayName}{isMulti && 's'}
                            </Label>
                            <Select
                                name={dv.tableName}
                                isMulti={isMulti}
                                isDisabled={isDisabled}
                                id={dv.tableName}
                                onChange={val => this.updateFilters(dv, val)}
                                value={this.getFiltersForSelect(dv)}
                                options={this.state[dv.listName].map(item => ({ label: item[dv.displayValue], value: item[dv.storeValue] }))}
                                placeholder={isMulti ? 'Select one or more...' : 'Select...'}
                            />
                        </div>
                    );
                })}
            </div>
        );
    }

    render() {
        const totalVariables = ['dataGroupName', 'questionText', 'possibleResponseName', 'executionTime'];
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
            } else {
                // just to quiet the console
                return {};
            }
        });

        const downOptions = notRowCols
            .concat(this.state.groupingVariable || [])
            .filter(v => v !== 'executionTime')
            .map(v => (
                { label: totalExpandedVariables.find(dv => dv.tableName === v).displayName, value: v }
            ));
        const acrossOptions = notRowCols
            .concat(this.state.sortingVariable || [])
            .filter(v => v !== 'questionText')
            .map(v => (
                { label: totalExpandedVariables.find(dv => dv.tableName === v).displayName, value: v }
            ));

        return (
            <div>
                <div style={{ display: 'flex', justifyContent: 'space-between' }}>
                    <Link to='/'>Home</Link>

                    <a href="#" onClick={e => { e.preventDefault(); this.reset() }}>Reset</a>
                </div>

                {this.instructionalRow()}

                <div style={{ display: 'flex', alignItems: 'center' }}>
                    <div style={{ flex: 1 }}>
                        <Label for="downVariable">
                            Down Variable
                        </Label>
                        <Select
                            name="downVariable"
                            id="downVariable"
                            onChange={val => this.updateTableVariable(
                                'groupingVariable',
                                totalExpandedVariables.find(dv => dv.tableName === this.state.groupingVariable),
                                totalExpandedVariables.find(dv => dv.tableName === val.value),
                                val.value
                            )}
                            value={downOptions.find(dop => dop.value === this.state.groupingVariable)}
                            options={downOptions}
                        />
                    </div>

                    <div style={{ flex: 1 }}>
                        <Label for="acrossVariable">
                            Across Variable
                        </Label>
                        <Select
                            name="acrossVariable"
                            id="acrossVariable"
                            onChange={val => this.updateTableVariable(
                                'sortingVariable',
                                totalExpandedVariables.find(dv => dv.tableName === this.state.sortingVariable),
                                totalExpandedVariables.find(dv => dv.tableName === val.value),
                                val.value
                            )}
                            value={acrossOptions.find(ao => ao.value === this.state.sortingVariable)}
                            options={acrossOptions}
                        />
                    </div>
                </div>

                {this.dropdownRow(totalExpandedVariables.slice(0, 2))}
                {this.dropdownRow(totalExpandedVariables.slice(2, 4))}

                {this.state.groupingVariable && this.state.sortingVariable && this.getNonEmptyFiltersCount() > 1 && (
                    <div>
                        <ResultsDataTable
                            filters={this.state.filters}
                            groupingVariable={this.state.groupingVariable}
                            sortingVariable={this.state.sortingVariable}
                        />
                    </div>
                )}
            </div>
        );
    }

    reset() {
        this.setState({
            setupComplete: false,
            sortingVariable: null,
            groupingVariable: null,
            filters: {}
        });
    }

    getNonEmptyFiltersCount() {
        // needs to move
        Object.filter = (obj, predicate) =>
            Object.fromEntries(Object.entries(obj).filter(predicate));

        return Object.keys(Object.filter(this.state.filters, ([key, value]) => value.length > 0)).length;
    }

    getFilters(dropdownFilterVariable) {
        return this.state.filters[dropdownFilterVariable.filterKey] ? this.state.filters[dropdownFilterVariable.filterKey] : null;
    }

    getFiltersForSelect(dropdownFilterVariable) {
        return this.state[dropdownFilterVariable.listName].map(item => ({ label: item[dropdownFilterVariable.displayValue], value: item[dropdownFilterVariable.storeValue] })).filter(o => this.getFilters(dropdownFilterVariable)?.includes(o.value));
    }

    updateFilters(dropdownFilterVariable, value) {
        const setValue = Array.isArray(value) ? value.map(v => v.value) : [value.value];

        this.setState(prevState => ({
            filters: { ...prevState.filters, [dropdownFilterVariable.filterKey]: setValue },
            setupComplete: prevState.setupComplete || this.getNonEmptyFiltersCount() > 0
        }));
    }

    updateTableVariable(variableName, currentDropdownVariable, dropdownFilterVariable, value) {
        this.setState(prevState => ({
            filters: dropdownFilterVariable !== null ? {
                ...prevState.filters,
                [dropdownFilterVariable.filterKey]: []
            } : prevState.filters,
            [variableName]: value,
        }));
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
