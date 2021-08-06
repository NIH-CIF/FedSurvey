import React, { Component } from 'react';
import { Label, Input } from 'reactstrap';
import _ from 'lodash';
import Select from 'react-select';

export class Advanced extends Component {
    static displayName = Advanced.name;

    constructor(props) {
        super(props);
        this.state = {
            questionExecutions: [],
            dataGroups: [],
            executions: [],
            possibleResponses: [],
            setupComplete: false
        };
    }

    componentDidMount() {
        this.props.addButton({
            onClick: this.props.reset,
            text: 'Reset'
        });

        this.populateDropdownData();
    }

    instructionalRow() {
        const downAcrossSelected = this.props.groupingVariable !== null && this.props.sortingVariable !== null;
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
                    const isMulti = this.props.groupingVariable === dv.tableName || this.props.sortingVariable === dv.tableName;
                    const isDisabled = isMulti ? this.props.nonEmptyFiltersCount < 2 : (this.props.groupingVariable === null || this.props.sortingVariable === null);

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
        const totalVariables = ['dataGroupName', 'questionId', 'possibleResponseName', 'executionTime'];
        const rowCols = [this.props.groupingVariable, this.props.sortingVariable];
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
            } else if (v === 'questionId') {
                return {
                    tableName: 'questionId',
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
            .concat(this.props.groupingVariable || [])
            .filter(v => v !== 'executionTime')
            .map(v => (
                { label: totalExpandedVariables.find(dv => dv.tableName === v).displayName, value: v }
            ));
        const acrossOptions = notRowCols
            .concat(this.props.sortingVariable || [])
            .filter(v => v !== 'questionId')
            .map(v => (
                { label: totalExpandedVariables.find(dv => dv.tableName === v).displayName, value: v }
            ));

        return (
            <div>
                {this.instructionalRow()}

                <div style={{ display: 'flex', alignItems: 'center' }}>
                    <div style={{ flex: 1 }}>
                        <Label for="downVariable">
                            Down Variable
                        </Label>
                        <Select
                            name="downVariable"
                            id="downVariable"
                            onChange={val => this.props.updateTableVariable(
                                'groupingVariable',
                                totalExpandedVariables.find(dv => dv.tableName === this.props.groupingVariable),
                                totalExpandedVariables.find(dv => dv.tableName === val.value),
                                val.value
                            )}
                            value={downOptions.find(dop => dop.value === this.props.groupingVariable) || ''}
                            options={downOptions}
                        />
                    </div>

                    <div style={{ flex: 1 }}>
                        <div style={{ display: 'flex', alignItems: 'flex-end' }}>
                            <div style={{ flex: 1 }}>
                                <Label for="acrossVariable">
                                    Across Variable
                                </Label>
                                <Select
                                    name="acrossVariable"
                                    id="acrossVariable"
                                    onChange={val => this.props.updateTableVariable(
                                        'sortingVariable',
                                        totalExpandedVariables.find(dv => dv.tableName === this.props.sortingVariable),
                                        totalExpandedVariables.find(dv => dv.tableName === val.value),
                                        val.value
                                    )}
                                    value={acrossOptions.find(ao => ao.value === this.props.sortingVariable) || ''}
                                    options={acrossOptions}
                                />
                            </div>

                            {(this.props.sortingVariable === 'executionTime' || this.props.sortingVariable === 'dataGroupName') && (
                                <div style={{ flex: 0, display: 'flex', flexDirection: 'column', marginLeft: 4 }}>
                                    &Delta;?

                                    <Input
                                        type="checkbox"
                                        style={{ margin: 0, position: 'static' }}
                                        onChange={e => this.props.updateShowDifference(e.target.checked)}
                                        checked={this.props.showDifference}
                                    />
                                </div>
                            )}
                        </div>
                    </div>
                </div>

                {this.dropdownRow(totalExpandedVariables.slice(0, 2))}
                {this.dropdownRow(totalExpandedVariables.slice(2, 4))}
            </div>
        );
    }

    getNonEmptyFiltersCount() {
        // needs to move
        Object.filter = (obj, predicate) =>
            Object.fromEntries(Object.entries(obj).filter(predicate));

        return Object.keys(Object.filter(this.props.filters, ([key, value]) => value.length > 0)).length;
    }

    getFilters(dropdownFilterVariable) {
        return this.props.filters[dropdownFilterVariable.filterKey] ? this.props.filters[dropdownFilterVariable.filterKey] : null;
    }

    getFiltersForSelect(dropdownFilterVariable) {
        return this.state[dropdownFilterVariable.listName].map(item => ({ label: item[dropdownFilterVariable.displayValue], value: item[dropdownFilterVariable.storeValue] })).filter(o => this.getFilters(dropdownFilterVariable)?.includes(o.value));
    }

    updateFilters(dropdownFilterVariable, value) {
        this.setState(prevState => ({
            setupComplete: prevState.setupComplete || this.props.nonEmptyFiltersCount > 0
        }));
        this.props.updateFilters(dropdownFilterVariable, value);
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

        const groupedQuestionExecutions = Object.entries(_.groupBy(questionExecutions, qe => qe.questionId)).map(([key, value]) => ({
            body: value[0].body,
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
