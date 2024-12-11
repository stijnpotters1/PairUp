import React, {ReactNode} from "react";

interface CardContainerProps {
    children: ReactNode;
}

const CardContainer: React.FunctionComponent<CardContainerProps> = ({children}) => {
    return (
        <div className="d-flex flex-column custom-card-container">
            {children}
        </div>
    );
}

export default CardContainer;