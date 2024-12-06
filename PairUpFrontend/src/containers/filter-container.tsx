import React, {ReactNode} from "react";

interface FilterContainerProps {
    children: ReactNode;
}

const FilterContainer: React.FunctionComponent<FilterContainerProps> = ({ children }) => {
    return (
        <div className="d-flex flex-column">
            {children}
        </div>
    );
}

export default FilterContainer;